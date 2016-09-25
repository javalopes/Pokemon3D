using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    [DataContract]
    public class BattleHooksModel : DataModel<BattleHooksModel>
    {
        [DataMember(Order = 0)]
        public BattleScriptHookModel[] ScriptHooks;

        public override object Clone()
        {
            var clone = MemberwiseClone() as BattleHooksModel;
            clone.ScriptHooks = ScriptHooks.Clone() as BattleScriptHookModel[];
            return clone;
        }
    }
}
