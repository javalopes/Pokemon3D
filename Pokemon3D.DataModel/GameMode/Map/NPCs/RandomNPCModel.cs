using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.GameMode.Map.NPCs
{
    [DataContract(Namespace = "")]
    public class RandomNPCModel : DataModel<RandomNPCModel>
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public int Chance;

        [DataMember(Order = 2)]
        public TextureSourceModel Texture;

        [DataMember(Order = 3)]
        public string ScriptBinding;

        [DataMember(Order = 4, Name = "Behaviour")]
        private string _behaviour;
        
        public RandomNPCBehaviour Behaviour
        {
            get { return ConvertStringToEnum<RandomNPCBehaviour>(_behaviour); }
            set { _behaviour = value.ToString(); }
        }

        public override object Clone()
        {
            var clone = (RandomNPCModel)MemberwiseClone();
            clone.Texture = Texture.CloneModel();
            return clone;
        }
    }
}
