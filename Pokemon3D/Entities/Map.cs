using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.Entities.System;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities
{
    class Map
    {
        private readonly MapModel _mapModel;
        private readonly World _world;
        private List<Entity> _allMapEntities;

        public Map(World world, MapModel mapModel)
        {
            _world = world;
            _mapModel = mapModel;
            _allMapEntities = new List<Entity>();
        }

        public void Load(Vector3 basicOffset)
        {
            if (_mapModel.Entities != null)
            {
                foreach (var entityDefinition in _mapModel.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        var entities = PlaceEntities(entityDefinition, entityPlacing, basicOffset);
                        _allMapEntities.AddRange(entities);
                    }
                }
            }

            var mapFragmentManager = GameInstance.GetService<GameModeManager>().ActiveGameMode.MapFragmentManager;

            if (_mapModel.Fragments != null)
            {
                foreach (var fragmentImport in _mapModel.Fragments)
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
        }

        public void Deactivate()
        {
            foreach(var entity in _allMapEntities)
            {
                entity.IsActive = false;
            }
        }

        public void Activate()
        {
            foreach (var entity in _allMapEntities)
            {
                entity.IsActive = true;
            }
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
                        X = MathHelper.ToDegrees(entityPlacing.Rotation.X),
                        Y = MathHelper.ToDegrees(entityPlacing.Rotation.Y),
                        Z = MathHelper.ToDegrees(entityPlacing.Rotation.Z)
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
