using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Common;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;

namespace Pokemon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// A singleton factory to create <see cref="EntityComponent"/> instances.
    /// </summary>
    class EntityComponentFactory : Singleton<EntityComponentFactory>
    {
        private EntityComponentFactory() { }
        
        /// <summary>
        /// Creates an empty <see cref="DataStorageEntityComponent"/> with the given name.
        /// </summary>
        public EntityComponent GetComponent(Entity parent, string name)
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
        public EntityComponent GetComponent(Entity parent, EntityComponentModel dataModel)
        {
            EntityComponent comp;

            var parameters = new EntityComponentDataCreationStruct
            {
                Parent = parent,
                Data = dataModel.Data,
                Name = dataModel.Id
            };

            switch (dataModel.Id)
            {
                case EntityComponent.IDs.Collision:
                    comp = new CollisionEntityComponent(parameters);
                    break;
                case EntityComponent.IDs.AnimateTextures:
                    comp = new AnimateTexturesEntityComponent(parameters);
                    break;
                default:
                    comp = new DataStorageEntityComponent(parameters);
                    break;
            }

            return comp;
        }
    }
}
