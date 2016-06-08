using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map.Entities
{
    /// <summary>
    /// The data model for an entity.
    /// </summary>
    [DataContract(Namespace = "")]
    public class EntityModel : DataModel<EntityModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Generator;
        
        [DataMember(Order = 2)]
        public EntityComponentModel[] Components;

        [DataMember(Order = 3)]
        public bool IsStatic;

        public override object Clone()
        {
            var clone = (EntityModel)MemberwiseClone();
            clone.Components = (EntityComponentModel[])Components.Clone();
            return clone;
        }
    }
}
