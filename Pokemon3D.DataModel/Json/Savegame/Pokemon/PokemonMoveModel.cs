using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame.Pokemon
{
    [DataContract]
    public class PokemonMoveModel : JsonDataModel<PokemonMoveModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public int CurrentPP;

        [DataMember(Order = 2)]
        public int MaxPP;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
