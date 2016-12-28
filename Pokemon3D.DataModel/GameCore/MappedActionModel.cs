using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameCore
{
    [DataContract(Namespace = "")]
    public class MappedActionModel : DataModel<MappedActionModel>
    {
        [DataMember(Order = 0, Name = "InputType")]
        private string _inputType;

        [DataMember(Order = 1)]
        public bool IsAxis;

        [DataMember(Order = 2)]
        public string AssingedValue;

        public InputType InputType
        {
            get { return ConvertStringToEnum<InputType>(_inputType); }
            set { _inputType = value.ToString(); }
        }

        public override object Clone()
        {
            return (MappedActionModel)MemberwiseClone();
        }
    }
}