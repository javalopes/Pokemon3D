using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame.Pokemon
{
    [DataContract]
    class CatchInfoModel : JsonDataModel<CatchInfoModel>
    {
        // where
        [DataMember(Order = 0)]
        public string Location;

        // how
        [DataMember(Order = 1)]
        public string Method;

        // who
        [DataMember(Order = 2)]
        public string TrainerName;

        // with what
        [DataMember(Order = 3)]
        public int BallItemId;

        public override object Clone()
        {
            var clone = (PokemonSaveModel)MemberwiseClone();
            return clone;
        }
    }
}
