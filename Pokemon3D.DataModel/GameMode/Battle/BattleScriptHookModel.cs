using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    [DataContract]
    public class BattleScriptHookModel : DataModel<BattleScriptHookModel>
    {
        [DataMember(Name = "HookType", Order = 0)]
        private string _hookType;

        public BattleHookType HookType
        {
            get { return ConvertStringToEnum<BattleHookType>(_hookType); }
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
