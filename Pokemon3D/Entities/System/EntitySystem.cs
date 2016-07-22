using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.Entities.System.Generators;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.Data;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities.System
{
    class EntitySystem
    {
        private readonly object _lockObject = new object();
        private readonly List<Entity> _entities;
        private readonly List<Entity> _entitiesToInitialize;

        public EntityGeneratorSupplier EntityGeneratorSupplier { get; }

        public EntitySystem()
        {
            _entities = new List<Entity>();
            _entitiesToInitialize = new List<Entity>();
            EntityGeneratorSupplier = new EntityGeneratorSupplier();
        }

        public Entity CreateEntityFromDataModel(EntityModel entityModel, EntityFieldPositionModel entityPlacing, Vector3 position, bool isInitializing = false)
        {
            var entity = CreateEntity(isInitializing);
            entity.Id = entityModel.Id;

            foreach (var compModel in entityModel.Components)
            {
                if (!entity.HasComponent(compModel.Id))
                {
                    entity.AddComponent(EntityComponentFactory.GetComponent(entity, compModel));
                }
            }

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

            return entity;
        }

        public int EntityCount
        {
            get { lock (_lockObject) return _entities.Count; }
        }

        public Entity CreateEntity(bool isInitializing = false)
        {
            Entity entity;
            lock (_lockObject)
            {
                entity = new Entity(isInitializing, OnEntityInitialized);

                if (entity.IsInitializing)
                {
                    _entitiesToInitialize.Add(entity);
                }
                else
                {
                    _entities.Add(entity);
                }
            }
            return entity;
        }

        public void InitializeAllPendingEntities()
        {
            lock (_lockObject)
            {
                for (var i = 0; i < _entitiesToInitialize.Count; i++)
                {
                    var entity = _entitiesToInitialize[i];
                    entity.EndInitializing(true);
                    _entities.Add(entity);
                }
                _entitiesToInitialize.Clear();
            }
        }

        private void OnEntityInitialized(Entity entity)
        {
            lock (_lockObject)
            {
                if (_entitiesToInitialize.Remove(entity)) _entities.Add(entity);
            }
        }

        public Entity GetEntity(string id)
        {
            Entity entity;
            lock (_lockObject)
            {
                entity = _entities.FirstOrDefault(e => e.Id == id);
            }
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            lock (_lockObject)
            {
                if (!_entitiesToInitialize.Remove(entity))
                {
                    _entities.Remove(entity);
                }
                entity.OnRemoved();
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_lockObject)
            {
                for (var i = 0; i < _entities.Count; i++)
                {
                    _entities[i].Update(gameTime);
                }
            }
        }

        public void MergeStaticVisualEntities(IList<Entity> entitiesToMerge)
        {
            var entitiesByCategory = entitiesToMerge.Where(
                e => e.IsStatic && e.ComponentCount == 1 && e.HasComponent<ModelEntityComponent>())
                .GroupBy(e => e.GetComponent<ModelEntityComponent>().Material.CompareId);

            foreach (var entityListToMerge in entitiesByCategory)
            {
                if (entityListToMerge.Count() == 1) continue;

                var entity = CreateEntity(true);

                var mergeData = entityListToMerge.Select(ConvertEntityToGeometryMerge);

                var mergedGeometry = GeometryData.Merge(mergeData);
                Mesh mergedMesh = null;

                GameInstance.EnsureExecutedInMainThread(() => mergedMesh = new Mesh(GameController.Instance.GraphicsDevice, mergedGeometry));
                var material = entityListToMerge.First().GetComponent<ModelEntityComponent>().Material;
                material.TexcoordOffset = Vector2.Zero;
                material.TexcoordScale = Vector2.One;

                entity.AddComponent(new ModelEntityComponent(entity, mergedMesh, material, false));

                foreach (var entityToRemove in entityListToMerge)
                {
                    RemoveEntity(entityToRemove);
                }
            }
        }

        private static GeometryDataMerge ConvertEntityToGeometryMerge(Entity entity)
        {
            var comp = entity.GetComponent<ModelEntityComponent>();
            return new GeometryDataMerge
            {
                Data = comp.Mesh.GeometryData,
                TextureOffset = comp.Material.TexcoordOffset,
                TextureScale = comp.Material.TexcoordScale,
                Transformation = entity.WorldMatrix
            };
        }
    }
}
