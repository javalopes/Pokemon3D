using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.Rendering;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Collisions;

namespace Pokemon3D.GameModes.Maps
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity : GameObject
    {
        private readonly List<EntityComponent> _components = new List<EntityComponent>();
        private Vector3 _position;
        private Vector3 _scale;

        public Scene Scene { get; }
                
        public Entity(EntitySystem system)
        {
            Scene = system.Scene;
        }

        /// <summary>
        /// Returns, if this entity is marked as static. When an entity is static, it cannot modify its position, rotation and scale.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Entity update method to update all of the entity's components.
        /// </summary>
        public void Update(float elapsedTime)
        {
            for (int i = 0; i < _components.Count; i++) _components[i].Update(elapsedTime);
        }

        public void RenderPreparations(Camera observer)
        {
            for (int i = 0; i < _components.Count; i++) _components[i].RenderPreparations(observer);
        }

        #region Components

        public void AddComponent(EntityComponent component)
        {
            if (!HasComponent(component.Name))
            {
                _components.Add(component);
                component.OnComponentAdded();
            }
        }

        public void RemoveComponent(string componentName)
        {
            if (HasComponent(componentName))
            {
                var component = GetComponent(componentName);
                _components.Remove(component);
                component.OnComponentRemove();
            }
        }

        /// <summary>
        /// Returns a component of this <see cref="Entity"/>.
        /// </summary>
        public EntityComponent GetComponent(string componentName)
        {
            return _components.FirstOrDefault(c => c.Name.Equals(componentName, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns a component of a specific type of this <see cref="Entity"/>.
        /// </summary>
        public T GetComponent<T>(string componentName) where T : EntityComponent
        {
            return GetComponent(componentName) as T;
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific name.
        /// </summary>
        public bool HasComponent(string componentName)
        {
            return _components.Any(c => (c.Name ?? "").Equals(componentName, System.StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific type and name.
        /// </summary>
        public bool HasComponent<T>(string componentName) where T : EntityComponent
        {
            var component = GetComponent(componentName);
            return component != null && component.GetType() == typeof(T);
        }

        #endregion
    }
}
