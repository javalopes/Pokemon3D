using System;

namespace Pokemon3D.ScriptPipeline.ApiClasses
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ApiClassAttribute : Attribute
    {
        public string ClassName { get; set; }
    }
}
