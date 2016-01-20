using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.StorageSystem
{
    [DataContract(Namespace = "")]
    public class StorageSystemModel : DataModel<StorageSystemModel>
    {
        [DataMember(Order = 0)]
        public StorageBoxModel[] Boxes;

        public override object Clone()
        {
            var clone = (StorageSystemModel)MemberwiseClone();
            clone.Boxes = (StorageBoxModel[])Boxes.Clone();
            return clone;
        }
    }
}
