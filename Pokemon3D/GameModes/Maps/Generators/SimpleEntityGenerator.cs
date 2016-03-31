using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps.Generators
{
    class SimpleEntityGenerator : EntityGenerator
    {
        public virtual IEnumerable<Entity> Generate(EntitySystem entitySystem, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            var list = new List<Entity>();
            list.Add(entitySystem.CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position));
            return list;
        }
    }
}
