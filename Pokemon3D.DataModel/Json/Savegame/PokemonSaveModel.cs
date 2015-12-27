using System;
using System.Runtime.Serialization;
using Pokemon3D.DataModel.Json.Pokemon;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.Savegame
{
    [DataContract]
    public class PokemonSaveModel : JsonDataModel<PokemonSaveModel>
    {
        public int Id;

        private string _gender;

        public PokemonGender Gender
        {
            get { return ConvertStringToEnum<PokemonGender>(_gender); }
            set { _gender = value.ToString(); }
        }

        public PokemonStatSetModel IVs;

        public bool IsShiny;

        public int AbilityId;

        public int NatureId;

        public string OT;
        
        public PokemonCatchInfo CatchInfo;

        public string PersonalityValue;

        public int HP;

        public int Experience;

        public string Nickname;

        public int Friendship;

        public HeldItemModel Item;

        private string _status;

        public PokemonStatus Status
        {
            get { return ConvertStringToEnum<PokemonStatus>(_status); }
            set { _status = value.ToString(); }
        }

        public PokemonStatSetModel EVs;

        public int EggSteps;

        public string AdditionalData;

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
