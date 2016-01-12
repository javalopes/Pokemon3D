using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Items.SpecialItems
{
    [DataContract(Namespace = "")]
    public class MegaStoneModel : DataModel<MegaStoneModel>
    {
        /// <summary>
        /// The number of the Pokémon in the Pokedex that this Mega Stone applies to.
        /// </summary>
        [DataMember]
        public int PokemonId;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
