using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map.Entities
{
    /// <summary>
    /// A model for an additional component assigned to an entity.
    /// </summary>
    [DataContract(Namespace = "")]
    public class EntityComponentModel : DataModel<EntityComponentModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public EntityComponentDataItemModel[] Data;

        public override object Clone()
        {
            var entityComponent = (EntityComponentModel)MemberwiseClone();
            entityComponent.Data = (EntityComponentDataItemModel[])Data.Clone();
            return entityComponent;
        }
    }
}
