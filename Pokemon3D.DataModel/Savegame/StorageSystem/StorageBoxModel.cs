using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.StorageSystem
{
    class StorageBoxModel : DataModel<StorageBoxModel>
    {
        [DataMember(Order = 0)]
        public StorageBoxStyleModel Style;

        [DataMember(Order = 1)]
        public StorageBoxEntryModel[] Pokemon;

        public override object Clone()
        {
            var clone = (StorageBoxModel)MemberwiseClone();
            clone.Style = Style.CloneModel();
            clone.Pokemon = (StorageBoxEntryModel[])Pokemon.Clone();
            return clone;
        }
    }
}
