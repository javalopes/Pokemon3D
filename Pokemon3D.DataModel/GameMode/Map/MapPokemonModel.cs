using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map
{
    [DataContract(Namespace = "")]
    public class MapPokemonModel : DataModel<MapPokemonModel>
    {
        [DataMember(Order = 0)]
        public bool ShowFollower;

        [DataMember(Order = 1)]
        public bool WildInGrass;

        [DataMember(Order = 3)]
        public bool WildOnFloor;

        [DataMember(Order = 2)]
        public bool WildInWater;

        [DataMember(Order = 4)]
        public int WildAbilityChance;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
