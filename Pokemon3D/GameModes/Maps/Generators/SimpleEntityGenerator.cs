using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.Json.GameMode.Map.Entities;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps.Generators
{
    class SimpleEntityGenerator : EntityGenerator
    {
        public List<Entity> Generate(Map map, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            return new List<Entity>() { new Entity(map, entityDefinition.Entity, entityPlacing, position) };
        }
    }
}
