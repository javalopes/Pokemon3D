using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using System;
using System.Reflection;
using Pokemon3D.Entities.System.Components;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// Factory to create <see cref="EntityComponent"/> instances.
    /// </summary>
    static class EntityComponentFactory
    {
        private static Dictionary<string, Type> _componentsByType;

        /// <summary>
        /// Creates an empty <see cref="DataStorageEntityComponent"/> with the given name.
        /// </summary>
        public static EntityComponent GetEmptyComponent(Entity parent, string name)
        {
            return new DataStorageEntityComponent(new EntityComponentDataCreationStruct
            {
                Name = name,
                Parent = parent
            });
        }

        /// <summary>
        /// Creates a new instance of an <see cref="EntityComponent"/>.
        /// </summary>
        public static EntityComponent GetComponent(Entity parent, EntityComponentModel dataModel)
        {
            EnsureComponentCreatorsAreLoaded();

            var parameters = new EntityComponentDataCreationStruct
            {
                Parent = parent,
                Data = dataModel.Data,
                Name = dataModel.Id
            };

            Type componentType;
            if(_componentsByType.TryGetValue(dataModel.Id.ToLowerInvariant(), out componentType))
            {
                return (EntityComponent)Activator.CreateInstance(componentType, parameters);
            }

            return new DataStorageEntityComponent(parameters);

        }

        private static void EnsureComponentCreatorsAreLoaded()
        {
            if (_componentsByType == null)
            {
                _componentsByType = new Dictionary<string, Type>();
                var entityComponentTypes = typeof(EntityComponent).Assembly.GetTypes()
                                                                           .Where(t => typeof(EntityComponent).IsAssignableFrom(t)
                                                                                       && !t.IsAbstract
                                                                                       && t.GetCustomAttributes<JsonComponentIdAttribute>().Any());

                foreach(var type in entityComponentTypes)
                {
                    var id = type.GetCustomAttribute<JsonComponentIdAttribute>().Id;
                    _componentsByType.Add(id, type);
                }
            }
        }
    }
}
