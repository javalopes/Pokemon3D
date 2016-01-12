using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps.Generators
{
    /// <summary>
    /// Classes to generate entities based on patterns from map files.
    /// </summary>
    interface EntityGenerator
    {
        List<Entity> Generate(Map map, EntityFieldModel entityDefinition, EntityFieldPositionModel entityPlacing, Vector3 position);
    }
}
