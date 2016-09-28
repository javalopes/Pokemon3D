using Pokemon3D.Scripting.Types;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An adapter to convert script objects to regular .Net objects.
    /// </summary>
    public static class ScriptOutAdapter
    {
        /// <summary>
        /// Translates an <see cref="SObject"/> to a dynamic type.
        /// </summary>
        public static object Translate(SObject obj)
        {
            // todo: C# 7: put a swtich statement type match instead of aweful if case blocks.
            if (obj is SBool)
            {
                return ((SBool)obj).Value;
            }
            else if (obj is SString)
            {
                return ((SString)obj).Value;
            }
            else if (obj is SNumber)
            {
                return TranslateNumber((SNumber)obj);
            }
            else if (obj is SArray)
            {
                return TranslateArray((SArray)obj);
            }
            else if (obj is SNull)
            {
                return null;
            }
            else if (obj is SUndefined)
            {
                return obj;
            }
            else if (obj is SFunction)
            {
                return TranslateFunction((SFunction)obj);
            }
            else if (obj is SError)
            {
                return TranslateError((SError)obj);
            }
            else if (obj is SUndefined)
            {
                return NetUndefined.Instance;
            }
            else if ((obj as SProtoObject)?.Prototype?.MappedType != null)
            {
                return Translate((SProtoObject)obj, ((SProtoObject)obj).Prototype.MappedType);
            }
            else if (obj is SProtoObject)
            {
                return TranslateDynamic((SProtoObject)obj);
            }
            else
            {
                return obj.ToScriptSource();
            }
        }

        private static object TranslateNumber(SNumber obj)
        {
            var value = obj.Value;

            if (Math.Abs(value % 1) < double.Epsilon)
                return (int)value;
            else
                return value;
        }
        
        private static object TranslateArray(SArray obj)
        {
            return obj.ArrayMembers.Select(Translate).ToArray();
        }

        private static object TranslateError(SError obj)
        {
            return new ScriptRuntimeException(obj);
        }

        private static object TranslateDynamic(SProtoObject obj)
        {
            var returnObj = new ExpandoObject() as IDictionary<string, object>;

            foreach (var item in obj.Members)
            {
                var memberName = item.Key;
                // Do not translate back the prototype and super instances:
                if (memberName != SProtoObject.MEMBER_NAME_PROTOTYPE &&
                    memberName != SProtoObject.MEMBER_NAME_SUPER)
                {
                    var memberContent = SObject.Unbox(item.Value);
                    returnObj.Add(memberName, Translate(memberContent));
                }
            }

            return returnObj;
        }

        private static object TranslateFunction(SFunction obj)
        {
            if (obj.Method != null)
                return obj.Method;
            else if (obj.DotNetMethod != null)
                return obj.DotNetMethod;
            else
                return obj.ToScriptSource();
        }

        /// <summary>
        /// Translates an <see cref="SObject"/> to a specific type.
        /// </summary>
        public static object Translate(SProtoObject obj, Type t)
        {
            var instance = Activator.CreateInstance(t);

            var fields = t
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                .ToArray();

            foreach (var field in fields)
            {
                var varAttr = field.GetCustomAttribute<ScriptVariableAttribute>(false);
                if (varAttr != null)
                {
                    var identifier = field.Name;
                    if (!string.IsNullOrEmpty(varAttr.VariableName))
                        identifier = varAttr.VariableName;

                    var setValue = SObject.Unbox(obj.Members[identifier]);

                    try
                    {
                        field.SetValue(instance, Translate(setValue));
                    }
                    catch (Exception)
                    {
                        // This is most likely a type binding issue: Set null if the types don't fit!
                        field.SetValue(instance, null);
                    }
                }
                else
                {
                    var refAttr = field.GetCustomAttribute<ReferenceAttribute>(false);
                    if (refAttr != null)
                    {
                        var identifier = field.Name;
                        if (!string.IsNullOrEmpty(refAttr.VariableName))
                            identifier = refAttr.VariableName;

                        field.SetValue(instance, obj.ReferenceContainer[identifier]);
                    }
                }
            }

            return instance;
        }
    }
}
