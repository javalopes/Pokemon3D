using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Pokemon
{
    [DataContract(Namespace = "")]
    public class PokemonSpriteSheetModel : DataModel<PokemonSpriteSheetModel>
    {
        [DataMember(Order = 0)]
        public string Source;

        [DataMember(Order = 1)]
        public SizeModel FrameSize;

        [DataMember(Order = 2)]
        public int FrameCount;

        public override object Clone()
        {
            var clone = (PokemonSpriteSheetModel)MemberwiseClone();
            clone.FrameSize = FrameSize.CloneModel();
            return clone;
        }
    }
}
