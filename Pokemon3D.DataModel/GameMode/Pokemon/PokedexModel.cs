using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Pokemon
{
    /// <summary>
    /// Contains information about a Pokédex in the game.
    /// </summary>
    [DataContract(Namespace = "")]
    public class PokedexModel : DataModel<PokedexModel>
    {
        /// <summary>
        /// The identification of this Pokédex that it is referred to.
        /// </summary>
        [DataMember(Order = 0)]
        public string Id;

        /// <summary>
        /// The list of Pokémon Ids that are contained in this Pokédex.
        /// </summary>
        public string[] PokemonIds;

        public override object Clone()
        {
            var clone = (PokedexModel)MemberwiseClone();
            clone.PokemonIds = (string[])PokemonIds.Clone();
            return clone;
        }
    }
}
