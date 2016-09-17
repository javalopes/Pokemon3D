using Pokemon3D.Scripting.Types;
using Pokemon3D.Scripting.Types.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An adapter to convert .Net objects to script objects.
    /// </summary>
    public static class ScriptInAdapter
    {
        /// <summary>
        /// Returns the "undefined" script object.
        /// </summary>
        public static SObject GetUndefined(ScriptProcessor processor)
        {
            return processor.Undefined;
        }

        /// <summary>
        /// Translates a .Net object to a script object.
        /// </summary>
        public static SObject Translate(ScriptProcessor processor, object objIn)
        {
            // todo: C# 7: put a swtich statement type match instead of aweful if case blocks.
            if (objIn == null)
            {
                return TranslateNull(processor);
            }

            if (objIn.GetType() == typeof(SObject) || objIn.GetType().IsSubclassOf(typeof(SObject)))
            {
                // this is already an SObject, return it.
                return (SObject)objIn;
            }

            if (objIn is sbyte || objIn is byte || objIn is short || objIn is ushort || objIn is int || objIn is uint || objIn is long || objIn is ulong || objIn is float || objIn is double)
            {
                return TranslateNumber(processor, Convert.ToDouble(objIn));
            }
            else if (objIn is string || objIn is char)
            {
                return TranslateString(processor, objIn.ToString()); // ToString will return just the string for the string type, and a string from a char.
            }
            else if (objIn is bool)
            {
                return TranslateBool(processor, (bool)objIn);
            }
            else if (objIn is Type)
            {
                return TranslatePrototype(processor, (Type)objIn);
            }
            else if (objIn.GetType().IsArray)
            {
                return TranslateArray(processor, (Array)objIn);
            }
            else if (objIn is BuiltInMethod || objIn is DotNetBuiltInMethod)
            {
                return TranslateFunction((Delegate)objIn);
            }
            else if (objIn is ScriptRuntimeException)
            {
                return TranslateException((ScriptRuntimeException)objIn);
            }
            else
            {
                return TranslateObject(processor, objIn);
            }
        }

        private static SObject TranslateNull(ScriptProcessor processor)
        {
            return processor.Null;
        }

        private static SObject TranslateNumber(ScriptProcessor processor, double dblIn)
        {
            return processor.CreateNumber(dblIn);
        }

        private static SObject TranslateString(ScriptProcessor processor, string strIn)
        {
            return processor.CreateString(strIn);
        }

        private static SObject TranslateBool(ScriptProcessor processor, bool boolIn)
        {
            return processor.CreateBool(boolIn);
        }

        private static SObject TranslateFunction(Delegate methodIn)
        {
            return new SFunction(methodIn);
        }

        private static SObject TranslateArray(ScriptProcessor processor, Array array) =>
            processor.Context.CreateInstance("Array", array.Cast<object>().Select((t, i) =>
                    Translate(processor, array.GetValue(i))).ToArray());

        private static SObject TranslateException(ScriptRuntimeException exceptionIn)
        {
            return exceptionIn.ErrorObject;
        }

        private static SObject TranslateObject(ScriptProcessor processor, object objIn)
        {
            var objType = objIn.GetType();

            var typeName = objType.Name;
            var customNameAttribute = objType.GetCustomAttribute<ScriptPrototypeAttribute>();
            if (!string.IsNullOrWhiteSpace(customNameAttribute?.VariableName))
                typeName = customNameAttribute.VariableName;

            Prototype prototype = null;

            if (processor.Context.IsPrototype(typeName))
                prototype = processor.Context.GetPrototype(typeName);
            else
                prototype = TranslatePrototype(processor, objIn.GetType());

            var obj = prototype.CreateInstance(processor, null, false);

            // Set the field values of the current instance:

            var fields = objIn.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false);

                foreach (var attr in attributes)
                {
                    if (attr.GetType() == typeof(ScriptVariableAttribute))
                    {
                        var memberAttr = (ScriptMemberAttribute)attr;

                        var identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(objIn);

                        obj.SetMember(identifier, Translate(processor, fieldContent));
                    }
                    else if (attr.GetType() == typeof(ScriptFunctionAttribute))
                    {
                        // When it's a field and a function, we have the source code of the function as value of the field.
                        // Example: public string MyFunction = "function() { console.log('Hello World'); }";

                        var memberAttr = (ScriptMemberAttribute)attr;

                        var identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var functionCode = field.GetValue(objIn).ToString();

                        obj.SetMember(identifier, new SFunction(processor, functionCode));
                    }
                }
            }

            return obj;
        }

        internal static Prototype TranslatePrototype(ScriptProcessor processor, Type t)
        {
            var name = t.Name;
            var customNameAttribute = t.GetCustomAttribute<ScriptPrototypeAttribute>();
            if (!string.IsNullOrWhiteSpace(customNameAttribute?.VariableName))
                name = customNameAttribute.VariableName;

            var prototype = new Prototype(name);

            object typeInstance = null;
            if (!t.IsAbstract)
            {
                typeInstance = Activator.CreateInstance(t);
            }

            var fields = t
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false);

                foreach (var attr in attributes)
                {
                    if (attr.GetType() == typeof(ScriptVariableAttribute))
                    {
                        var memberAttr = (ScriptMemberAttribute)attr;
                        var identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(typeInstance);

                        if (fieldContent == null)
                            prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined, field.IsStatic, field.IsInitOnly, false, false));
                        else
                            prototype.AddMember(processor, new PrototypeMember(identifier, Translate(processor, fieldContent), field.IsStatic, field.IsInitOnly, false, false));
                    }
                    else if (attr.GetType() == typeof(ScriptFunctionAttribute))
                    {
                        var memberAttr = (ScriptFunctionAttribute)attr;
                        var identifier = field.Name;
                        if (!string.IsNullOrEmpty(memberAttr.VariableName))
                            identifier = memberAttr.VariableName;

                        var fieldContent = field.GetValue(typeInstance);

                        if (fieldContent == null)
                        {
                            if (memberAttr.FunctionType != ScriptFunctionType.Standard)
                                throw new InvalidOperationException("A member function marked with Indexer Set, Indexer Get or Constructor has to be defined.");

                            prototype.AddMember(processor, new PrototypeMember(identifier, processor.Undefined, field.IsStatic, field.IsInitOnly, false, false));
                        }
                        else
                        {
                            switch (memberAttr.FunctionType)
                            {
                                case ScriptFunctionType.Standard:
                                    prototype.AddMember(processor, new PrototypeMember(identifier, new SFunction(processor, fieldContent.ToString()), field.IsStatic, field.IsInitOnly, false, false));
                                    break;
                                case ScriptFunctionType.IndexerGet:
                                    prototype.IndexerGetFunction = new SFunction(processor, fieldContent.ToString());
                                    break;
                                case ScriptFunctionType.IndexerSet:
                                    prototype.IndexerSetFunction = new SFunction(processor, fieldContent.ToString());
                                    break;
                                case ScriptFunctionType.Constructor:
                                    prototype.Constructor = new PrototypeMember(Prototype.CLASS_METHOD_CTOR, new SFunction(processor, fieldContent.ToString()), false, true, false, false);
                                    break;
                            }
                        }
                    }
                }
            }

            var methods = t
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ScriptFunctionAttribute>(false);
                if (attr != null)
                {
                    var identifier = method.Name;
                    if (!string.IsNullOrEmpty(attr.VariableName))
                        identifier = attr.VariableName;

                    if (attr.FunctionType == ScriptFunctionType.Getter)
                        identifier = SProtoObject.PROPERTY_GET_PREFIX + identifier;
                    if (attr.FunctionType == ScriptFunctionType.Setter)
                        identifier = SProtoObject.PROPERTY_SET_PREFIX + identifier;

                    Delegate methodDelegate = null;

                    if (method.GetParameters().Length == 3)
                    {
                        // two parameter means the method is a DotNetBuiltInMethod.
                        methodDelegate = (DotNetBuiltInMethod)Delegate.CreateDelegate(typeof(DotNetBuiltInMethod), method);
                    }
                    else if (method.GetParameters().Length == 4)
                    {
                        // four parameters means that the method is a valid BuiltInMethod.
                        methodDelegate = (BuiltInMethod)Delegate.CreateDelegate(typeof(BuiltInMethod), method);
                    }

                    switch (attr.FunctionType)
                    {
                        case ScriptFunctionType.Standard:
                        case ScriptFunctionType.Getter:
                        case ScriptFunctionType.Setter:
                            prototype.AddMember(processor, new PrototypeMember(identifier, new SFunction(methodDelegate), attr.IsStatic, true, false, false));
                            break;
                        case ScriptFunctionType.IndexerGet:
                            prototype.IndexerGetFunction = new SFunction(methodDelegate);
                            break;
                        case ScriptFunctionType.IndexerSet:
                            prototype.IndexerSetFunction = new SFunction(methodDelegate);
                            break;
                        case ScriptFunctionType.Constructor:
                            prototype.Constructor = new PrototypeMember(Prototype.CLASS_METHOD_CTOR, new SFunction(methodDelegate), false, true, false, false);
                            break;
                    }
                }
            }

            prototype.MappedType = t;

            return prototype;
        }
    }
}
