using System;

namespace Pokemon3D.Scripting.Adapters
{
    /// <summary>
    /// An attribute to add to fields that should get added as variables in adapted script objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ScriptVariableAttribute : ScriptMemberAttribute
    { }
}
