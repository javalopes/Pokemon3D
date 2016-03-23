using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps.Generators
{
    class SimpleEntityGenerator : EntityGenerator
    {
        public virtual IEnumerable<Entity> Generate(EntitySystem entitySystem, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            yield return entitySystem.CreateEntityFromDataModel(entityDefinition.Entity, entityPlacing, position);
        }
    }
}
