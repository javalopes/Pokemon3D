using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps
{
    class Map : GameObject
    {
        private readonly List<Entity> _allEntities = new List<Entity>();
        private readonly MapModel _mapModel;

        public GameMode GameMode { get; }

        public Map(GameMode gameMode, MapModel mapModel)
        {
            GameMode = gameMode;
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
                    GameMode.MapFragmentManager.LoadFragmentAsync(fragmentImport.Id, l => FinishLoadingMapFragment(fragmentImport, l));
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

                        _allEntities.AddRange(generator.Generate(Game.EntitySystem, entityDefinition, entityPlacing, position));
                    }
                }
            }
        }

        public void Update(float elapsedTime)
        {
            _allEntities.ForEach(e => e.Update(elapsedTime));
        }

        public void AddEntity(Entity entity)
        {
            _allEntities.Add(entity);
        }

        public void RenderPreparations(Camera observer)
        {
            _allEntities.ForEach(e => e.RenderPreparations(observer));
        }
    }
}
