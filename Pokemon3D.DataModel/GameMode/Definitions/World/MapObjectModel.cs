using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions.World
{
    /// <summary>
    /// A base data model for the map objects.
    /// </summary>
    [DataContract(Namespace = "")]
    public abstract class MapObjectModel : DataModel<MapObjectModel>
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public Vector2Model Position;

        [DataMember(Order = 2)]
        public string[] Mapfiles;

        [DataMember(Order = 3)]
        public FlyToModel FlyTo;
    }
}
