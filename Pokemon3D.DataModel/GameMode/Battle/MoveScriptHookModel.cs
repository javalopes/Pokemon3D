using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    [DataContract(Namespace = "")]
    public class MoveScriptHookModel : DataModel<MoveScriptHookModel>
    {
        [DataMember(Name = "HookType", Order = 0)]
        private string _hookType;

        public MoveHookType HookType
        {
            get { return ConvertStringToEnum<MoveHookType>(_hookType); }
            set { _hookType = value.ToString(); }
        }

        [DataMember(Order = 1)]
        public string Script;

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
