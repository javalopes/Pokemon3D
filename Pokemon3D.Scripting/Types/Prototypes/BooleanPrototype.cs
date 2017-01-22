namespace Pokemon3D.Scripting.Types.Prototypes
{
    internal class BooleanPrototype : Prototype
    {
        public BooleanPrototype() : base("Boolean")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(ConstructorCall));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SBool();
        }

        private static SObject ConstructorCall(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var obj = (SBool)instance;
            obj.Value = (parameters[0] as SBool)?.Value ?? parameters[0].ToBool(processor).Value;

            return obj;
        }
    }
}
