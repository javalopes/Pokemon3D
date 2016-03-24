using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;
using Pokemon3D.GameModes.Maps.Generators;
using Pokemon3D.Rendering;
using System.Collections.Generic;
using System.Linq;

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
                var modelComponent = new ModelEntityComponent(entity, entityModel.RenderMode);
                entity.AddComponent(modelComponent);


                entity.Scale = entityPlacing.Scale?.GetVector3() ?? Vector3.One;

                if (entityPlacing.Rotation != null)
                {
                    if (entityPlacing.CardinalRotation)
                    {
                        entity.EulerAngles = new Vector3
                        {
                            X = entityPlacing.Rotation.X * MathHelper.PiOver2,
                            Y = entityPlacing.Rotation.Y * MathHelper.PiOver2,
                            Z = entityPlacing.Rotation.Z * MathHelper.PiOver2
                        };
                    }
                    else
                    {
                        entity.EulerAngles = new Vector3
                        {
                            X = MathHelper.ToDegrees(entityPlacing.Rotation.X),
                            Y = MathHelper.ToDegrees(entityPlacing.Rotation.Y),
                            Z = MathHelper.ToDegrees(entityPlacing.Rotation.Z)
                        };
                    }
                }
                else
                {
                    entity.EulerAngles = Vector3.Zero;
                }

                entity.Position = position;
            }

            if (entityModel.Components.Any(c =>EntityComponent.IDs.CollisionOffset.Equals(c.Id, System.StringComparison.OrdinalIgnoreCase)))
            {
                var size = TypeConverter.Convert<Vector3>(entityModel.Components.First(c => EntityComponent.IDs.CollisionSize.Equals(c.Id, System.StringComparison.OrdinalIgnoreCase)).Data);
                var offset = TypeConverter.Convert<Vector3>(entityModel.Components.First(c => EntityComponent.IDs.CollisionOffset.Equals(c.Id, System.StringComparison.OrdinalIgnoreCase)).Data);
                entity.AddComponent(new CollisionEntityComponent(entity, size, offset));
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

        public Entity CreateEntity(Entity parent = null)
        {
            var entity = new Entity(this);
            if (parent != null) parent.AddChild(entity);
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
