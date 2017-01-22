using System;

namespace Pokemon3D.Entities.System.Components
{
    /// <summary>
    /// A component to store data in, the default component.
    /// </summary>
    [JsonComponentId("data")]
    internal class DataStorageEntityComponent : EntityComponent
    {
        public DataStorageEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        { }

        // Don't add any logic/additional members to this class.
        public override EntityComponent Clone(Entity target)
        {
            throw new InvalidOperationException("Component cannot be cloned");
        }
    }
}
