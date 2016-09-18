using System.Linq;

namespace Pokemon3D.Scripting.Types.Prototypes
{
    internal class ArrayPrototype : Prototype
    {
        public ArrayPrototype() : base("Array")
        {
            Constructor = new PrototypeMember("constructor", new SFunction(constructor));
        }

        protected override SProtoObject CreateBaseObject()
        {
            return new SArray();
        }

        private static SObject constructor(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var arr = (SArray)instance;
            if (parameters.Length == 1 && parameters[0].TypeOf() == LITERAL_TYPE_NUMBER)
            {
                var length = (int)((SNumber)parameters[0]).Value;

                if (length >= 0)
                {
                    arr.ArrayMembers = new SObject[length];
                    for (var i = 0; i < length; i++)
                    {
                        arr.ArrayMembers[i] = processor.Undefined;
                    }
                }
                else
                {
                    arr.ArrayMembers = new SObject[0];
                }
                return arr;
            }
            else
            {
                arr.ArrayMembers = parameters;
                return arr;
            }
        }

        [BuiltInMethod(IsIndexerSet = true, MethodName = "IndexerSet")]
        public static SObject IndexerSet(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var arr = (SArray)instance;

            if (parameters.Length >= 2)
            {
                var accessor = (int)parameters[0].ToNumber(processor).Value;

                if (accessor >= 0 && accessor < arr.ArrayMembers.Length)
                {
                    arr.ArrayMembers[accessor] = parameters[1];
                }
            }

            return processor.Undefined;
        }

        [BuiltInMethod(IsIndexerGet = true, MethodName = "indexerGet")]
        public static SObject IndexerGet(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var arr = (SArray)instance;

            if (parameters.Length >= 1)
            {
                var accessor = (int)parameters[0].ToNumber(processor).Value;

                if (accessor >= 0 && accessor < arr.ArrayMembers.Length)
                {
                    return arr.ArrayMembers[accessor];
                }
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.PropertyGetter, MethodName = "length")]
        public static SObject Length(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var arr = (SArray)instance;
            return processor.CreateNumber(arr.ArrayMembers.Length);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "includes")]
        public static SObject Includes(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length >= 2)
            {
                var arr = (SArray)instance;
                var compare = parameters[0];
                var comparer = (SFunction)Unbox(parameters[1]);

                return processor.CreateBool(arr.ArrayMembers.Any(m => ((SBool)comparer.Call(processor, This, This, new[] { m, compare })).Value));
            }
            
            if (parameters.Length >= 1)
            {
                var arr = (SArray)instance;
                var compare = parameters[0];

                return processor.CreateBool(arr.ArrayMembers.Any(m => ObjectComparer.LooseEquals(processor, m, compare)));
            }

            return processor.Undefined;
        }
    }
}
