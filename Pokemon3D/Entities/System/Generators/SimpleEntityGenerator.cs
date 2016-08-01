using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using System.Collections.Generic;

namespace Pokemon3D.Entities.System.Generators
{
    internal class SimpleEntityGenerator : EntityGenerator
    {
        public virtual IEnumerable<Entity> Generate(EntitySystem entitySystem, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            return new List<Entity>
            {
                entitySystem.CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position)
            };
        }
    }
}
