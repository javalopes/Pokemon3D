using System;

namespace Pokemon3D.Entities.System
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false, Inherited = false)]
    public class JsonComponentIdAttribute : Attribute
    {
        public string Id { get; private set; }

        public JsonComponentIdAttribute(string id)
        {
            if (id.ToLowerInvariant() != id) throw new InvalidOperationException("ids needs to be lower case.");
            Id = id;
        }
    }
}
