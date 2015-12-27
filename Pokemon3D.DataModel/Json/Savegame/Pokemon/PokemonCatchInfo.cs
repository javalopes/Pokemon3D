using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame.Pokemon
{
    [DataContract]
    public class PokemonCatchInfo : JsonDataModel<PokemonCatchInfo>
    {
        [DataMember(Order = 0)]
        public string Location;

        [DataMember(Order = 1)]
        public string TrainerName;

        [DataMember(Order = 2)]
        public string BallItemId;

        [DataMember(Order = 3)]
        public string Method;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
