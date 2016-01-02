using Pokemon3D.DataModel.Json.Pokemon;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame.Pokemon
{
    /// <summary>
    /// Holds saved information about a Pokémon instance.
    /// </summary>
    [DataContract]
    public class PokemonSaveModel : DataModel<PokemonSaveModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1, Name = "Gender")]
        private string _gender;

        public PokemonGender Gender
        {
            get { return ConvertStringToEnum<PokemonGender>(_gender); }
            set { _gender = value.ToString(); }
        }

        [DataMember(Order = 2)]
        public PokemonStatSetModel IVs;

        [DataMember(Order = 3)]
        public bool IsShiny;

        [DataMember(Order = 4)]
        public string AbilityId;

        [DataMember(Order = 5)]
        public string NatureId;

        [DataMember(Order = 6)]
        public PokemonCatchInfo CatchInfo;

        [DataMember(Order = 7)]
        public string PersonalityValue;

        [DataMember(Order = 8)]
        public int HP;

        [DataMember(Order = 9)]
        public int Experience;

        [DataMember(Order = 10)]
        public string Nickname;

        [DataMember(Order = 11)]
        public int Friendship;

        [DataMember(Order = 12)]
        public HeldItemModel Item;

        [DataMember(Order = 13, Name = "Status")]
        private string _status;

        public PokemonStatus Status
        {
            get { return ConvertStringToEnum<PokemonStatus>(_status); }
            set { _status = value.ToString(); }
        }

        [DataMember(Order = 14)]
        public PokemonStatSetModel EVs;

        [DataMember(Order = 15)]
        public int EggSteps;

        [DataMember(Order = 16)]
        public string AdditionalData;

        [DataMember(Order = 17)]
        public PokemonMoveModel[] Moves;

        public override object Clone()
        {
            var clone = (PokemonSaveModel)MemberwiseClone();
            clone.IVs = IVs.CloneModel();
            clone.CatchInfo = CatchInfo.CloneModel();
            clone.EVs = EVs.CloneModel();
            clone.Item = Item.CloneModel();
            clone.Moves = (PokemonMoveModel[])Moves.Clone();
            return clone;
        }
    }
}
