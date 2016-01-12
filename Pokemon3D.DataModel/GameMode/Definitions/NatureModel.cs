using Pokemon3D.DataModel.Pokemon;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Definitions
{
    [DataContract(Namespace = "")]
    public class NatureModel : DataModel<NatureModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2, Name = "StatIncrease")]
        private string[] _statIncrease;

        public PokemonStatType[] StatIncrease
        {
            get { return ConvertStringCollectionToEnumCollection<PokemonStatType>(_statIncrease); }
            set { _statIncrease = ConvertEnumCollectionToStringCollection(value); }
        }

        [DataMember(Order = 3, Name = "StatDecrease")]
        private string[] _statDecrease;

        public PokemonStatType[] StatDecrease
        {
            get { return ConvertStringCollectionToEnumCollection<PokemonStatType>(_statDecrease); }
            set { _statDecrease = ConvertEnumCollectionToStringCollection(value); }
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
