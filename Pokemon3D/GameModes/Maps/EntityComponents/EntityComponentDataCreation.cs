using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel.GameMode.Map.Entities;

namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A helper struct to easily move construction parameters around.
    /// </summary>
    struct EntityComponentDataCreationStruct
    {
        public string Name;
        public EntityComponentDataItem[] Data;
        public Entity Parent;
    }
}
