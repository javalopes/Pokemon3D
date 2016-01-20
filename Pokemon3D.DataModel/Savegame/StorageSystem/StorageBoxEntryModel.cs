using System.Runtime.Serialization;
using Pokemon3D.DataModel.Savegame.Pokemon;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.StorageSystem
{
    [DataContract(Namespace = "")]
    public class StorageBoxEntryModel : DataModel<StorageBoxEntryModel>
    {
        [DataMember(Order = 0)]
        public Vector2Model Position;

        [DataMember(Order = 1)]
        public PokedexSaveModel Pokemon;

        public override object Clone()
        {
            var clone = (StorageBoxEntryModel)MemberwiseClone();
            clone.Position = Position.CloneModel();
            clone.Pokemon = Pokemon.CloneModel();
            return clone;
        }
    }
}
