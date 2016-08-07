using System.Runtime.Serialization;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    [DataContract(Namespace = "")]
    public class AbilityModel : DataModel<AbilityModel>
    {
        [DataMember(Order = 0)]
        public string Id;

        [DataMember(Order = 1)]
        public string Name;

        [DataMember(Order = 2)]
        public string Description;

        [DataMember(Order = 3)]
        public AbilityScriptHookModel[] ScriptHooks;

        public override object Clone()
        {
            var clone = (AbilityModel)MemberwiseClone();
            clone.ScriptHooks = (AbilityScriptHookModel[])ScriptHooks.Clone();
            return clone;
        }
    }
}
