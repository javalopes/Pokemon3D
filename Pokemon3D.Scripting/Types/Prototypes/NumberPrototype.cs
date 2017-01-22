namespace Pokemon3D.Scripting.Types.Prototypes
{
    internal class NumberPrototype : Prototype
    {
        public NumberPrototype() : base("Number")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(ConstructorCall));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SNumber();
        }

        private static SObject ConstructorCall(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var obj = (SNumber)instance;

            var number = parameters[0] as SNumber;
            obj.Value = number?.Value ?? parameters[0].ToNumber(processor).Value;

            return obj;
        }
    }
}
