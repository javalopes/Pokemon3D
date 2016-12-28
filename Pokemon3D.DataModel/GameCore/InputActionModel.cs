using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameCore
{
    [DataContract(Namespace = "")]
    public class InputActionModel : DataModel<InputActionModel>
    {
        [DataMember(Order = 0)]
        public string Name;

        [DataMember(Order = 1)]
        public MappedActionModel[] ActionsModel;

        public override object Clone()
        {
            var clone = (InputActionModel)MemberwiseClone();
            clone.ActionsModel = (MappedActionModel[]) ActionsModel.Clone();
            return clone;
        }
    }
}