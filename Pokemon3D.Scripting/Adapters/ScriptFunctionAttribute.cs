using System;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An attribute to add to methods that should get added as variables in adapted script objects.
    /// 
    /// The signature of the method has to conform to <see cref="BuiltInMethod"/>.
    /// 
    /// This can also be applied to string fields, creating source functions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
    public class ScriptFunctionAttribute : ScriptMemberAttribute
    {
        public ScriptFunctionAttribute(ScriptFunctionType functionType)
        {
            FunctionType = functionType;
        }

        internal bool IsStatic { get; set; }

        internal ScriptFunctionType FunctionType { get; set; }
    }
}
