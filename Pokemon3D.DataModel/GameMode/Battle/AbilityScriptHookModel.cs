using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    [DataContract(Namespace = "")]
    public class AbilityScriptHookModel : DataModel<AbilityScriptHookModel>
    {
        [DataMember(Name = "HookType", Order = 0)]
        private string _hookType;

        public AbilityHookType HookType
        {
            get { return ConvertStringToEnum<AbilityHookType>(_hookType); }
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
