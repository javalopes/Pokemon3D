using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.Pokemon
{
    /// <summary>
    /// An entry for the Pokédex in the save file.
    /// </summary>
    [DataContract(Namespace = "")]
    public class PokedexEntrySaveModel : DataModel<PokedexEntrySaveModel>
    {
        /// <summary>
        /// The Id of the Pokémon.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;
        
        [DataMember(Order = 1, Name = "EntryType")]
        private string _entryType;

        /// <summary>
        /// The entry type of the Pokémon in the Pokédex.
        /// </summary>
        public PokedexEntryType EntryType
        {
            get { return ConvertStringToEnum<PokedexEntryType>(_entryType); }
            set { _entryType = value.ToString(); }
        }

        /// <summary>
        /// The forms of the Pokémon that have been (at least) seen.
        /// </summary>
        [DataMember(Order = 2)]
        public string[] Forms;

        public override object Clone()
        {
            var clone = (PokedexEntrySaveModel)MemberwiseClone();
            clone.Forms = (string[])Forms.Clone();
            return clone;
        }
    }
}
