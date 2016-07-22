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

        public Map(World world, MapModel mapModel)
        {
            _world = world;
            _mapModel = mapModel;
        }

        public void Load()
        {
            if (_mapModel.Entities != null)
            {
                foreach (var entityDefinition in _mapModel.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        PlaceEntities(entityDefinition, entityPlacing, Vector3.Zero);
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
                        var fragmentOffset = position.GetVector3();

                        foreach (var entityDefinition in importedFragment.Entities)
                        {
                            foreach (var entityPlacing in entityDefinition.Placing)
                            {
                                entitiesToMerge.AddRange(PlaceEntities(entityDefinition, entityPlacing, fragmentOffset));
                            }
                        }

                        _world.EntitySystem.MergeStaticVisualEntities(entitiesToMerge);
                        entitiesToMerge.Clear();
                    }
                }
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
            for (var x = 1.0f; x <= entityPlacing.Size.X; x += entityPlacing.Steps.X)
            {
                for (var y = 1.0f; y <= entityPlacing.Size.Y; y += entityPlacing.Steps.Y)
                {
                    for (var z = 1.0f; z <= entityPlacing.Size.Z; z += entityPlacing.Steps.Z)
                    {
                        var position = entityPlacing.Position.GetVector3() + new Vector3(x, y, z) + offset;

                        entities.Add(CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position));
                    }
                }
            }
            return entities;
        }

    }
}
