using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Savegame.Pokemon
{
    /// <summary>
    /// Contains the save information about a Pokédex.
    /// </summary>
    [DataContract(Namespace = "")]
    public class PokedexSaveModel : DataModel<PokedexSaveModel>
    {
        /// <summary>
        /// The Id of the Pokédex.
        /// </summary>
        [DataMember(Order = 0)]
        public string PokedexId;

        /// <summary>
        /// The entries for seen / caught Pokémon in this Pokédex.
        /// </summary>
        [DataMember(Order = 1)]
        public PokedexEntrySaveModel[] Entries;

        public override object Clone()
        {
            var clone = (PokedexSaveModel)MemberwiseClone();
            clone.Entries = (PokedexEntrySaveModel[])Entries.Clone();
            return clone;
        }
    }
}
