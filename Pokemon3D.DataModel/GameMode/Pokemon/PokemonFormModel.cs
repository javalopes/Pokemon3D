using Pokemon3D.DataModel.Pokemon;
using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Pokemon
{
    [DataContract(Namespace = "")]
    public class PokemonFormModel : DataModel<PokemonFormModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public PokemonFormActivationModel Activation;

        [DataMember(Order = 2)]
        public PokemonStatSetModel BaseStats;

        [DataMember(Order = 3)]
        public TextureSourceModel MenuTexture;

        [DataMember(Order = 4)]
        public TextureSourceModel SpriteFrontTexture;

        [DataMember(Order = 5)]
        public TextureSourceModel SpriteBackTexture;

        [DataMember(Order = 6)]
        public TextureSourceModel OverworldTexture;

        [DataMember(Order = 7)]
        public string Type1;

        [DataMember(Order = 8)]
        public string Type2;

        [DataMember(Order = 9)]
        public int CatchRate;

        [DataMember(Order = 10)]
        public string[] Abilities;

        /// <summary>
        /// If this member is set, the move pools are taken from the referenced form instead of this one.
        /// </summary>
        [DataMember(Order = 11)]
        public string ShareMovesWithForm;

        [DataMember(Order = 12)]
        public LevelUpMoveModel[] LevelMoves;

        [DataMember(Order = 13)]
        public string[] LearnableMoves;
        
        public override object Clone()
        {
            var clone = (PokemonFormModel)MemberwiseClone();
            clone.Activation = Activation.CloneModel();
            clone.BaseStats = BaseStats.CloneModel();
            clone.MenuTexture = MenuTexture.CloneModel();
            clone.SpriteFrontTexture = SpriteFrontTexture.CloneModel();
            clone.SpriteBackTexture = SpriteBackTexture.CloneModel();
            clone.OverworldTexture = OverworldTexture.CloneModel();
            clone.Abilities = (string[])Abilities.Clone();
            clone.LevelMoves = (LevelUpMoveModel[])LevelMoves.Clone();
            clone.LearnableMoves = (string[])LearnableMoves.Clone();
            return clone;
        }
    }
}
