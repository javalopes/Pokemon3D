using System.Runtime.Serialization;
using Pokemon3D.DataModel.General;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map
{
    /// <summary>
    /// The battle map data for this map. Can be null at runtime.
    /// </summary>
    [DataContract(Namespace = "")]
    public class BattleMapDataModel : DataModel<BattleMapDataModel>
    {
        [DataMember]
        public string BattleMapFile;

        [DataMember]
        public Vector3Model CameraPosition;

        public override object Clone()
        {
            var clone = (BattleMapDataModel)MemberwiseClone();
            clone.CameraPosition = CameraPosition.CloneModel();
            return clone;
        }
    }
}
