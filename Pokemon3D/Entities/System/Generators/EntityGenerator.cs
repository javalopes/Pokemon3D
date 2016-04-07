using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using System.Collections.Generic;

namespace Pokemon3D.Entities.System.Generators
{
    /// <summary>
    /// Classes to generate entities based on patterns from map files.
    /// </summary>
    interface EntityGenerator
    {
        IEnumerable<Entity> Generate(EntitySystem entitySystem, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position);
    }
}
