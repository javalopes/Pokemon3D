using System;

namespace Pokemon3D.ScriptPipeline.APIClasses
{
    [AttributeUsage(AttributeTargets.Class)]
    class APIClassAttribute : Attribute
    {
        public string ClassName { get; set; }
    }
}
