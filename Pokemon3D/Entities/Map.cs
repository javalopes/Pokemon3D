using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.Entities.System;
using static Pokemon3D.GameCore.GameProvider;
using System.IO;
using Pokemon3D.Common.FileSystem;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;

namespace Pokemon3D.Entities
{
    internal class Map
    {
        private readonly World _world;
        private readonly List<Entity> _allMapEntities;
        private readonly string _dataPath;
        private readonly string _id;

        public MapModel Model { get; private set; }
        public bool IsActive { get; private set; }

        public Vector3 Offset { get; private set; }

        public Map(World world, string id)
        {
            _world = world;
            _id = id;
            _allMapEntities = new List<Entity>();
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            _dataPath = gameMode.GetMapFilePath(_id);
        }

        public void Load(Vector3 basicOffset, bool forceResetCache = false)
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            Model = gameMode.LoadMap(_id);

            Offset = basicOffset;
            if (Model.Entities != null)
            {
                foreach (var entityDefinition in Model.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        var entities = PlaceEntities(entityDefinition, entityPlacing, basicOffset);
                        _allMapEntities.AddRange(entities);
                    }
                }
            }

            var mapFragmentManager = GameInstance.GetService<GameModeManager>().ActiveGameMode.MapFragmentManager;

            if (Model.Fragments != null)
            {
                foreach (var fragmentImport in Model.Fragments)
                {
                    var importedFragment = mapFragmentManager.GetFragment(fragmentImport.Id);
                    var positions = fragmentImport.Positions;
                    var entitiesToMerge = new List<Entity>();

                    foreach (var position in positions)
                    {
                        var fragmentOffset = position.GetVector3() + basicOffset;

                        foreach (var entityDefinition in importedFragment.Entities)
                        {
                            foreach (var entityPlacing in entityDefinition.Placing)
                            {
                                entitiesToMerge.AddRange(PlaceEntities(entityDefinition, entityPlacing, fragmentOffset));
                            }
                        }

                        var merged = _world.EntitySystem.MergeStaticVisualEntities(entitiesToMerge);
                        _allMapEntities.AddRange(merged);
                        entitiesToMerge.Clear();
                    }
                }
            }
            IsActive = true;

            if (GameInstance.GetService<GameConfiguration>().EnableFileHotSwapping)
            {
                FileObserver.Instance.StartFileObserve(_dataPath, MapChanged);
            }
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            foreach(var entity in _allMapEntities)
            {
                entity.IsActive = false;
            }

            if (GameInstance.GetService<GameConfiguration>().EnableFileHotSwapping)
            {
                FileObserver.Instance.StopFileObserve(_dataPath, MapChanged);
            }
            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive) return;
            foreach (var entity in _allMapEntities)
            {
                entity.IsActive = true;
            }
            if (GameInstance.GetService<GameConfiguration>().EnableFileHotSwapping)
            {
                FileObserver.Instance.StartFileObserve(_dataPath, MapChanged);
            }
            IsActive = true;
        }

        private void MapChanged(global::System.Object sender, FileSystemEventArgs e)
        {
            var gameMode = GameInstance.GetService<GameModeManager>().ActiveGameMode;
            var mapModel = gameMode.LoadMap(_id, true);
            foreach (var entity in _allMapEntities)
            {
                _world.EntitySystem.RemoveEntity(entity);
            }
            _allMapEntities.Clear();
            Load(Offset, true);
            _world.EntitySystem.InitializeAllPendingEntities();
        }

        private Entity CreateEntityFromDataModel(EntityModel entityModel, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            var entity = _world.EntitySystem.CreateEntity(true);
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

        private List<Entity> PlaceEntities(EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 offset)
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

    }
}
