using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering;

namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A component of an <see cref="Entity"/>, responsible for the Entity's functionality.
    /// </summary>
    partial class EntityComponent
    {
        /// <summary>
        /// The original name of this component.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The raw data of this component.
        /// </summary>
        public Dictionary<string,string> Data { get; set; }

        /// <summary>
        /// The owning parent <see cref="Entity"/> of this component.
        /// </summary>
        protected Entity Parent { get; }

        protected EntityComponent(EntityComponentDataCreationStruct parameters)
        {
            Name = parameters.Name;
            Data = parameters.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            Parent = parameters.Parent;
        }

        protected EntityComponent(Entity parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Updates this property's logic.
        /// </summary>
        public virtual void Update(float elapsedTime) { }

        /// <summary>
        /// Prepares this entity component for the render.
        /// </summary>
        public virtual void RenderPreparations(Camera observer) { }

        /// <summary>
        /// Converts the data string that came with the component data into the desired data type.
        /// </summary>
        public T GetData<T>(string key)
        {
            return TypeConverter.Convert<T>(Data[key]);
        }

        public virtual void OnComponentAdded() { }

        public virtual void OnComponentRemove() { }

        #region Behaviour

        /// <summary>
        /// Gets executed when the player interacts with the entity.
        /// </summary>
        public virtual void Click() { }

        /// <summary>
        /// The player is about to collide with the entity.
        /// </summary>
        public virtual FunctionResponse Collision() { return FunctionResponse.NoValue; }

        #endregion
    }
}
