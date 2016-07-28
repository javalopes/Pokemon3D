namespace Pokemon3D.Scripting.Types.Prototypes
{
    internal class StringPrototype : Prototype
    {
        public StringPrototype(ScriptProcessor processor) : base("String")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(constructor));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SString();
        }

        private static SObject constructor(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var obj = (SString)instance;

            if (parameters[0] is SString)
            {
                obj.Value = ((SString)parameters[0]).Value;
                obj.Escaped = ((SString)parameters[0]).Escaped;
            }
            else
                obj.Value = parameters[0].ToString(processor).Value;

            return obj;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.PropertyGetter, MethodName = "length")]
        public static SObject Length(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var obj = (SString)instance;

            return processor.CreateNumber(obj.Value.Length);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.PropertyGetter, IsStatic = true, MethodName = "empty")]
        public static SObject Empty(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            return processor.CreateString("");
        }
    }
}
