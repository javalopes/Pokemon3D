using System;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// Add this attribute to any class that derives from <see cref="EntityComponent"/> to mark it with its name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class JsonComponentIdAttribute : Attribute
    {
        public string Id { get; private set; }

        public JsonComponentIdAttribute(string id)
        {
            if (id.ToLowerInvariant() != id) throw new InvalidOperationException("ids needs to be lower case.");
            Id = id;
        }
    }
}
