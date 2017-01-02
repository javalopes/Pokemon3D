using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.UI;
using Pokemon3D.Entities.System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using static GameProvider;
using Pokemon3D.Common.Input;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameModes;

namespace Pokemon3D.Entities
{
    internal class World
    {
        private readonly Dictionary<string, List<Entity>> _entityTemplatesByFragmentId = new Dictionary<string, List<Entity>>();
        private readonly InputSystem _inputSystem;
        private readonly NotificationBar _notificationBar;
        private readonly Dictionary<string, Map> _allMaps = new Dictionary<string, Map>();

        public Player Player { get; private set; }
        public Inventory Inventory { get; private set; }

        public EntitySystem EntitySystem { get; }

        public World()
        {
            EntitySystem = new EntitySystem();
            _inputSystem = GameInstance.GetService<InputSystem>();
            _notificationBar = GameInstance.GetService<NotificationBar>();
        }

        public void StartNewGameAsync(Action onFinished)
        {
            GameInstance.ExecuteBackgroundJob(LoadWorldAsync, onFinished);
        }

        private void LoadWorldAsync()
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            gameMode.Preload();
            
            Player = new Player(this);
            Inventory = new Inventory(this);
            ActivateMapsWithOffsets(gameMode.GameModeInfo.StartMap, Vector3.Zero);
            EntitySystem.InitializeAllPendingEntities();
        }

        private void ActivateMapsWithOffsets(string id, Vector3 position)
        {
            var mainMap = ActivateMap(id, position);
            
            if (mainMap.Model.OffsetMaps != null)
            {
                foreach(var offsetModel in mainMap.Model.OffsetMaps)
                {
                    ActivateMap(offsetModel.MapFile, position + offsetModel.Offset.GetVector3());
                }
            }
        }

        private Map ActivateMap(string id, Vector3 position)
        {
            Map existingMap;
            if (_allMaps.TryGetValue(id, out existingMap))
            {
                existingMap.Activate();
                return existingMap;
            }
            else
            {
                var map = new Map(this, id);
                map.Load(position);
                _allMaps.Add(id, map);
                return map;
            }
        }

        public void LoadMap(string id, double x, double y, double z)
        {
            foreach(var map in _allMaps) map.Value.Deactivate();
            ActivateMapsWithOffsets(id, new Vector3((float)x, (float)y, (float)z));

            var mapsToUnload = _allMaps.Where(m => !m.Value.IsActive).ToArray();
            foreach(var mapToUnload in mapsToUnload)
            {
                mapToUnload.Value.Unload();
                _allMaps.Remove(mapToUnload.Key);
            }
            
            EntitySystem.InitializeAllPendingEntities();
        }

        public List<Entity> CreateEntitiesFromFragment(MapFragmentModel model, Vector3 position)
        {
            List<Entity> entityTemplates;
            List<Entity> createdEntities;
            
            if (_entityTemplatesByFragmentId.TryGetValue(model.Id, out entityTemplates))
            {
                createdEntities = entityTemplates.Select(e => EntitySystem.CreateInstance(e, true)).ToList();
            }
            else
            {
                var entitiesToMerge = new List<Entity>();
                foreach (var entityDefinition in model.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        entitiesToMerge.AddRange(PlaceEntities(entityDefinition, entityPlacing, Vector3.Zero));
                    }
                }

                if(entitiesToMerge.Count > 0)
                {
                    var merged = EntitySystem.MergeStaticVisualEntities(entitiesToMerge);
                    if(merged.Count < entitiesToMerge.Count)
                    {
                        _entityTemplatesByFragmentId.Add(model.Id, merged.Select(Entity.CreateTemplate).ToList());
                        createdEntities = merged;
                    }
                    else
                    {
                        createdEntities = entitiesToMerge;
                    }
                }
                else
                {
                    createdEntities = entitiesToMerge;
                }
            }

            createdEntities.ForEach(e => e.Position = position);
            return createdEntities;
        }

        public void Update(GameTime gameTime)
        {
            EntitySystem.Update(gameTime);
            Inventory.Update(gameTime);

            if (_inputSystem.KeyboardHandler.IsKeyDownOnce(Keys.V))
            {
                if (Player.MovementMode == PlayerMovementMode.GodMode)
                {
                    _notificationBar.PushNotification(NotificationKind.Information, "Disabled God Mode");
                }
                Player.MovementMode = Player.MovementMode == PlayerMovementMode.FirstPerson ? PlayerMovementMode.ThirdPerson : PlayerMovementMode.FirstPerson;
            }

            if (_inputSystem.KeyboardHandler.IsKeyDownOnce(Keys.F10))
            {
                Player.MovementMode = PlayerMovementMode.GodMode;
                _notificationBar.PushNotification(NotificationKind.Information, "Enabled God Mode");
            }
        }

        public List<Entity> PlaceEntities(EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 offset)
        {
            var entities = new List<Entity>();
            for (var x = 0; x < (int)entityPlacing.Size.X; x++)
            {
                for (var y = 0; y < (int)entityPlacing.Size.Y; y++)
                {
                    for (var z = 0; z < (int)entityPlacing.Size.Z; z++)
                    {
                        var position = entityPlacing.Position.GetVector3() + new Vector3(x * entityPlacing.Steps.X, y * entityPlacing.Steps.Y, z * entityPlacing.Steps.Z) + offset;

                        entities.Add(CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position));
                    }
                }
            }
            return entities;
        }

        private Entity CreateEntityFromDataModel(EntityModel entityModel, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            var entity = EntitySystem.CreateEntity(true);
            entity.Id = entityModel.Id;
            entity.IsStatic = entityModel.IsStatic;

            entity.Scale = entityPlacing.Scale?.GetVector3() ?? Vector3.One;

            if (entityPlacing.Rotation != null)
            {
                if (entityPlacing.CardinalRotation)
                {
                    entity.EulerAngles = new Vector3
                    {
                        X = entityPlacing.Rotation.X * MathHelper.PiOver2,
                        Y = entityPlacing.Rotation.Y * MathHelper.PiOver2,
                        Z = entityPlacing.Rotation.Z * MathHelper.PiOver2
                    };
                }
                else
                {
                    entity.EulerAngles = new Vector3
                    {
                        X = MathHelper.ToRadians(entityPlacing.Rotation.X),
                        Y = MathHelper.ToRadians(entityPlacing.Rotation.Y),
                        Z = MathHelper.ToRadians(entityPlacing.Rotation.Z)
                    };
                }
            }
            else
            {
                entity.EulerAngles = Vector3.Zero;
            }

            entity.Position = position;

            foreach (var compModel in entityModel.Components)
            {
                if (!entity.HasComponent(compModel.Id))
                {
                    entity.AddComponent(EntityComponentFactory.GetComponent(entity, compModel));
                }
            }

            return entity;
        }

        public void OnEarlyDraw()
        {
            Inventory.DrawInventory();
        }
    }
}
