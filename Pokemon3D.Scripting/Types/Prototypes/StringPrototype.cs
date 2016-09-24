using System;
using System.Linq;

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
            var str = instance as SString;

            return processor.CreateNumber(str.Value.Length);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.PropertyGetter, IsStatic = true, MethodName = "empty")]
        public static SObject Empty(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            return processor.CreateString("");
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "charAt")]
        public static SObject CharAt(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var str = (instance as SString).Value;

            if (str == "")
                return processor.CreateString("");

            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                var index = (int)(parameters[0] as SNumber).Value;

                if (index < 0 || index >= str.Length)
                    return processor.CreateString("");

                return processor.CreateString(str[index].ToString());
            }
            else
            {
                return processor.CreateString(str[0].ToString());
            }
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "concat")]
        public static SObject Concat(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length > 0)
            {
                var str = instance as SString;

                string result = str.Value;
                foreach (var param in parameters)
                {
                    string concatStr = param.ToString(processor).Value;
                    result += concatStr;
                }

                return processor.CreateString(result);
            }

            return processor.CreateString((instance as SString).Value);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "includes")]
        public static SObject Includes(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (parameters.Length >= 1)
            {
                var str = instance as SString;
                var includes = parameters[0].ToString(processor);

                return processor.CreateBool(str.Value.Contains(includes.Value));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "endsWith")]
        public static SObject EndsWith(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                var str = instance as SString;
                var includes = parameters[0] as SString;

                if (includes.Value == "")
                    return processor.CreateBool(str.Value == "");

                return processor.CreateBool(str.Value.EndsWith(includes.Value));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "startsWith")]
        public static SObject StartsWith(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                var str = instance as SString;
                var includes = parameters[0] as SString;

                if (includes.Value == "")
                    return processor.CreateBool(str.Value == "");

                return processor.CreateBool(str.Value.StartsWith(includes.Value));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "indexOf")]
        public static SObject IndexOf(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                var str = instance as SString;
                var search = parameters[0] as SString;

                if (!str.Value.Contains(search.Value) || search.Value == "")
                    return processor.CreateNumber(-1);

                return processor.CreateNumber(str.Value.IndexOf(search.Value));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "lastIndexOf")]
        public static SObject LastIndexOf(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                var str = instance as SString;
                var search = parameters[0] as SString;

                if (!str.Value.Contains(search.Value) || search.Value == "")
                    return processor.CreateNumber(-1);

                return processor.CreateNumber(str.Value.LastIndexOf(search.Value));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "padEnd")]
        public static SObject PadEnd(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var str = (instance as SString).Value;
            int totalLength = 0;
            var padStr = " ";

            if (TypeContract.Ensure(parameters, new[] { typeof(SNumber), typeof(SString) }))
            {
                totalLength = (int)(parameters[0] as SNumber).Value;
                padStr = (parameters[1] as SString).Value;
            }
            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                totalLength = (int)(parameters[0] as SNumber).Value;
            }

            if (padStr == "" || totalLength <= str.Length)
                return processor.CreateString(str);

            while (str.Length < totalLength)
                str += padStr;

            if (str.Length > totalLength)
                str = str.Remove(totalLength);

            return processor.CreateString(str);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "padStart")]
        public static SObject PadStart(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            var str = (instance as SString).Value;
            int totalLength = 0;
            var padStr = " ";
            string newPadded = "";

            if (TypeContract.Ensure(parameters, new[] { typeof(SNumber), typeof(SString) }))
            {
                totalLength = (int)(parameters[0] as SNumber).Value;
                padStr = (parameters[1] as SString).Value;
            }
            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                totalLength = (int)(parameters[0] as SNumber).Value;
            }

            if (padStr == "" || totalLength <= str.Length)
                return processor.CreateString(str);

            while (newPadded.Length + str.Length < totalLength)
                newPadded += padStr;

            if (newPadded.Length + str.Length > totalLength)
                newPadded = newPadded.Remove(totalLength - str.Length);

            str = newPadded + str;

            return processor.CreateString(str);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "repeat")]
        public static SObject Repeat(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                var repeatStr = (instance as SString).Value;
                var times = (int)(parameters[0] as SNumber).Value;
                var str = "";

                for (int i = 0; i < times; i++)
                    str += repeatStr;

                return processor.CreateString(str);
            }

            return processor.CreateString((instance as SString).Value);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "replace")]
        public static SObject Replace(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(SString), typeof(SString) }))
            {
                var str = (instance as SString).Value;
                var replace = (parameters[0] as SString).Value;
                var with = (parameters[1] as SString).Value;

                if (string.IsNullOrWhiteSpace(replace))
                    return processor.CreateString(str);

                return processor.CreateString(str.Replace(replace, with));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "slice")]
        public static SObject Slice(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(SNumber), typeof(SNumber) }))
            {
                var str = (instance as SString).Value;
                var sliceBegin = (int)(parameters[0] as SNumber).Value;
                var sliceEnd = (int)(parameters[1] as SNumber).Value;

                if (sliceBegin < 0)
                    sliceBegin += str.Length;
                if (sliceBegin < 0)
                    sliceBegin = 0;
                if (sliceBegin > str.Length)
                    return processor.CreateString("");

                if (sliceEnd < 0)
                    sliceEnd += str.Length;
                if (sliceEnd < 0)
                    sliceEnd = 0;
                if (sliceEnd > str.Length)
                    sliceEnd = str.Length;

                return processor.CreateString(str.Substring(sliceBegin, sliceEnd - sliceBegin));
            }
            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                var str = (instance as SString).Value;
                var sliceBegin = (int)(parameters[0] as SNumber).Value;

                if (sliceBegin < 0)
                    sliceBegin += str.Length;
                if (sliceBegin < 0)
                    sliceBegin = 0;
                if (sliceBegin > str.Length)
                    return processor.CreateString("");

                return processor.CreateString(str.Substring(sliceBegin));
            }

            return processor.Undefined;
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "split")]
        public static SObject Split(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            int limit = -1;
            string[] split;
            string[] delimiters;
            string str = (instance as SString).Value;

            if (TypeContract.Ensure(parameters, new[] { typeof(SArray), typeof(SNumber) }))
            {
                delimiters = (parameters[0] as SArray).ArrayMembers.Select(m => m.ToString(processor).Value).ToArray();
                limit = (int)(parameters[1] as SNumber).Value;
            }
            else if (TypeContract.Ensure(parameters, typeof(SArray)))
            {
                delimiters = (parameters[0] as SArray).ArrayMembers.Select(m => m.ToString(processor).Value).ToArray();
            }
            else if (TypeContract.Ensure(parameters, new[] { typeof(SString), typeof(SNumber) }))
            {
                delimiters = new[] { (parameters[0] as SString).Value };
                limit = (int)(parameters[1] as SNumber).Value;
            }
            else if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                delimiters = new[] { (parameters[0] as SString).Value };
            }
            else
            {
                return processor.CreateArray(new[] { processor.CreateString(str) });
            }

            split = str.Split(delimiters, StringSplitOptions.None);
            if (limit >= 0 && split.Length > limit)
            {
                var result = new string[limit];
                Array.Copy(split, result, limit);
                split = result;
            }

            return processor.CreateArray(split.Select(m => processor.CreateString(m)).ToArray());
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "toLower")]
        public static SObject ToLower(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            string str = (instance as SString).Value;
            return processor.CreateString(str.ToLower());
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "toUpper")]
        public static SObject ToUpper(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            string str = (instance as SString).Value;
            return processor.CreateString(str.ToUpper());
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "trim")]
        public static SObject Trim(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            string str = (instance as SString).Value;
            string[] trimChars = { " " };

            if (TypeContract.Ensure(parameters, typeof(SArray)))
            {
                trimChars = (parameters[0] as SArray).ArrayMembers.Select(m => m.ToString(processor).Value).ToArray();
            }
            else if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                trimChars = new[] { (parameters[0] as SString).Value };
            }

            foreach (var trimChar in trimChars)
            {
                while (str.StartsWith(trimChar))
                    str = str.Remove(0, trimChar.Length);
                while (str.EndsWith(trimChar))
                    str = str.Remove(str.Length - trimChar.Length);
            }

            return processor.CreateString(str);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "trimStart")]
        public static SObject TrimStart(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            string str = (instance as SString).Value;
            string[] trimChars = { " " };

            if (TypeContract.Ensure(parameters, typeof(SArray)))
            {
                trimChars = (parameters[0] as SArray).ArrayMembers.Select(m => m.ToString(processor).Value).ToArray();
            }
            else if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                trimChars = new[] { (parameters[0] as SString).Value };
            }

            foreach (var trimChar in trimChars)
            {
                while (str.StartsWith(trimChar))
                    str = str.Remove(0, trimChar.Length);
            }

            return processor.CreateString(str);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "trimEnd")]
        public static SObject TrimEnd(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            string str = (instance as SString).Value;
            string[] trimChars = { " " };

            if (TypeContract.Ensure(parameters, typeof(SArray)))
            {
                trimChars = (parameters[0] as SArray).ArrayMembers.Select(m => m.ToString(processor).Value).ToArray();
            }
            else if (TypeContract.Ensure(parameters, typeof(SString)))
            {
                trimChars = new[] { (parameters[0] as SString).Value };
            }

            foreach (var trimChar in trimChars)
            {
                while (str.EndsWith(trimChar))
                    str = str.Remove(str.Length - trimChar.Length);
            }

            return processor.CreateString(str);
        }

        [BuiltInMethod(FunctionType = FunctionUsageType.Default, MethodName = "remove")]
        public static SObject Remove(ScriptProcessor processor, SObject instance, SObject This, SObject[] parameters)
        {
            if (TypeContract.Ensure(parameters, new[] { typeof(SNumber), typeof(SNumber) }))
            {
                var str = (instance as SString).Value;
                var start = (int)(parameters[0] as SNumber).Value;
                var length = (int)(parameters[1] as SNumber).Value;

                if (start < 0)
                    start = 0;
                if (start > str.Length)
                    return processor.CreateString(str);

                if (length <= 0)
                    return processor.CreateString(str);
                if (length + start > str.Length)
                    length = str.Length - start;

                return processor.CreateString(str.Remove(start, length));
            }
            if (TypeContract.Ensure(parameters, typeof(SNumber)))
            {
                var str = (instance as SString).Value;
                var start = (int)(parameters[0] as SNumber).Value;
                
                if (start < 0)
                    start = 0;
                if (start > str.Length)
                    return processor.CreateString(str);

                return processor.CreateString(str.Remove(start));
            }

            return processor.Undefined;
        }
    }
}
