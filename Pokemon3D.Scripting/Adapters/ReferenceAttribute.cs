using System;

namespace Pokemon3D.Scripting.Adapters
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReferenceAttribute : ScriptMemberAttribute
    { }
}
