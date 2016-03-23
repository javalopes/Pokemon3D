using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;
using Pokemon3D.GameModes.Maps.Generators;
using Pokemon3D.Rendering;
using System.Collections.Generic;

namespace Pokemon3D.GameModes.Maps
{
    class EntitySystem
    {
        private readonly List<Entity> _entities;
        private readonly Scene _scene;
        
        public EntityGeneratorSupplier EntityGeneratorSupplier { get; }

        public EntitySystem(Scene scene)
        {
            _entities = new List<Entity>();
            _scene = scene;
            EntityGeneratorSupplier = new EntityGeneratorSupplier();
        }

        public Entity CreateEntityFromDataModel(EntityModel entityModel, EntityFieldPositionModel entityPlacing, Vector3 position)
        {
            var entity = CreateEntity();

            if (entityModel.RenderMode != null)
            {
                entity.AddComponent(new ModelEntityComponent(entity, entityModel.RenderMode));
            }

            foreach (var compModel in entityModel.Components)
            {
                if (!entity.HasComponent(compModel.Id))
                {
                    var comp = EntityComponentFactory.Instance.GetComponent(entity, compModel);
                    entity.AddComponent(comp);
                }
            }

            return entity;
        }

        public Entity CreateEntity()
        {
            var entity = new Entity(this);
            _entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public Scene Scene { get { return _scene; } }

        public int EntityCount => _entities.Count;

        public void Update(float elapsedTime)
        {
            _entities.ForEach(e => e.Update(elapsedTime));
        }
    }
}
