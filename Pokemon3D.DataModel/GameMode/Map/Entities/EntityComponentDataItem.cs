using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map.Entities
{
    /// <summary>
    /// Containing Data for entity component.
    /// </summary>
    [DataContract(Namespace = "")]
    public class EntityComponentDataItem : DataModel<EntityComponentDataItem>
    {
        [DataMember(Order = 0)]
        public string Key;

        [DataMember(Order = 1)]
        public string Value;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}