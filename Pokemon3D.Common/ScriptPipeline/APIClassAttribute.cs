using System;

namespace Pokemon3D.Common.ScriptPipeline
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ApiClassAttribute : Attribute
    {
        public string ClassName { get; set; }
    }
}
