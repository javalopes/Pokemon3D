using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameCore;
using System.Linq;
using Pokemon3D.GameModes.Maps.EntityComponents;

namespace Pokemon3D.GameModes.Maps
{
    class Map : GameObject
    {
        private readonly MapModel _mapModel;
        private World _world;

        public Map(World world, MapModel mapModel)
        {
            _world = world;
            _mapModel = mapModel;

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
            if (_mapModel.Fragments != null)
            {
                foreach (var fragmentImport in _mapModel.Fragments)
                {
                    Game.ActiveGameMode.MapFragmentManager.LoadFragmentAsync(fragmentImport.Id, l => FinishLoadingMapFragment(fragmentImport, l));
                }
            }
        }

        private void FinishLoadingMapFragment(MapFragmentImportModel importModel, MapFragmentModel fragmentModel)
        {
            var positions = importModel.Positions;

            foreach (var position in positions)
            {
                var fragmentOffset = position.GetVector3();

                foreach (var entityDefinition in fragmentModel.Entities)
                {
                    foreach (var entityPlacing in entityDefinition.Placing)
                    {
                        PlaceEntities(entityDefinition, entityPlacing, fragmentOffset);
                    }
                }
            }
        }

        private void CreateEntityFromDataModel(EntityModel entityModel, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            var entity = Game.EntitySystem.CreateEntity();
            entity.IsActive = false;

            foreach (var compModel in entityModel.Components)
            {
                if (!entity.HasComponent(compModel.Id))
                {
                    entity.AddComponent(EntityComponentFactory.GetComponent(entity, compModel));
                }
            }

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

            _world.AddEntityToActivate(entity);
        }

        private void PlaceEntities(EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 offset)
        {
            var generator = Game.EntitySystem.EntityGeneratorSupplier.GetGenerator(entityDefinition.Entity.Generator);
            for (var x = 1.0f; x <= entityPlacing.Size.X; x += entityPlacing.Steps.X)
            {
                for (var y = 1.0f; y <= entityPlacing.Size.Y; y += entityPlacing.Steps.Y)
                {
                    for (var z = 1.0f; z <= entityPlacing.Size.Z; z += entityPlacing.Steps.Z)
                    {
                        var position = entityPlacing.Position.GetVector3() + new Vector3(x, y, z) + offset;

                        CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position);
                    }
                }
            }
        }

    }
}
