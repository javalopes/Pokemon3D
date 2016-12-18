using System;
using Microsoft.Xna.Framework;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.Entities.System.Generators;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.Data;
using static GameProvider;

namespace Pokemon3D.Entities.System
{
    internal class EntitySystem
    {
        private readonly object _lockObject = new object();
        private readonly List<Entity> _entities;
        private readonly List<Entity> _entitiesToInitialize;

        public EntityGeneratorSupplier EntityGeneratorSupplier { get; }

        /// <summary>
        /// Creates a new empty entity system.
        /// </summary>
        public EntitySystem()
        {
            _entities = new List<Entity>();
            _entitiesToInitialize = new List<Entity>();
            EntityGeneratorSupplier = new EntityGeneratorSupplier();
        }

        public Entity GetPendingEntityById(string id)
        {
            return _entitiesToInitialize.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Creates an entity reading json data model. It also attaches all known entity components.
        /// </summary>
        /// <param name="entityModel">Deserialized entity model.</param>
        /// <param name="entityPlacing">Placement of entity.</param>
        /// <param name="position">position of entity.</param>
        /// <param name="isInitializing">If the entity should be created in initializing mode. This needs to end initializing.</param>
        /// <returns>Created entity instance.</returns>
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

        /// <summary>
        /// Counts of total entities.
        /// </summary>
        public int EntityCount
        {
            get { lock (_lockObject) return _entities.Count; }
        }

        /// <summary>
        /// Creates a new, empty entity.
        /// </summary>
        /// <param name="isInitializing">Create in initializing mode.</param>
        /// <param name="addComponents">Delegate for adding components on the fly.</param>
        /// <returns>New empty entity.</returns>
        public Entity CreateEntity(bool isInitializing = false, Func<Entity, EntityComponent[]> addComponents = null)
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

            var components = addComponents?.Invoke(entity);

            if (components?.Length > 0)
            {
                foreach (var component in components) entity.AddComponent(component);
            }

            return entity;
        }

        /// <summary>
        /// All entities created in initializing mode are activated.
        /// </summary>
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

        /// <summary>
        /// Gets an entity by id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Returns entity or null if not found.</returns>
        public Entity GetEntity(string id)
        {
            Entity entity;
            lock (_lockObject)
            {
                entity = _entities.FirstOrDefault(e => e.Id == id);
            }
            return entity;
        }

        /// <summary>
        /// Removes entity from system.
        /// </summary>
        /// <param name="entity"></param>
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

        /// <summary>
        /// Updates all entities.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            lock (_lockObject)
            {
                for(var i = 0; i < _entities.Count; i++)
                {
                    _entities[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Merges a list of entities. These entities must be static and contain a visual component only.
        /// </summary>
        /// <param name="entitiesToMerge">Entites to merge.</param>
        /// <returns>Merged list of entities.</returns>
        public List<Entity> MergeStaticVisualEntities(IList<Entity> entitiesToMerge)
        {
            var entitiesToMergeList = entitiesToMerge.Where(IsVisualEntityToMerge).ToArray();
            var entitiesNotToMerge = entitiesToMerge.Except(entitiesToMergeList).ToArray();
            var entitiesByCategory = entitiesToMergeList.GroupBy(e => e.GetComponent<ModelEntityComponent>().Material.CompareId);

            var mergedEntities = new List<Entity>();

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

                mergedEntities.Add(entity);
            }

            return mergedEntities.Concat(entitiesNotToMerge).ToList();
        }

        private static bool IsVisualEntityToMerge(Entity entity)
        {
            return entity.IsStatic && entity.ComponentCount == 1 && entity.HasComponent<ModelEntityComponent>();
        }

        /// <summary>
        /// Creates a copy of an entity. This will be marked as a template for instanciation.
        /// Template entities are not hold by the entity system.
        /// </summary>
        /// <param name="entityToTemplate">Entity to create template of</param>
        /// <returns>New entity which is marked as template</returns>
        public Entity CreateTemplate(Entity entityToTemplate)
        {
            Entity template;
            lock (_lockObject)
            {
                template = Entity.CreateTemplate(entityToTemplate);
            }
            return template;
        }

        public Entity CreateInstance(Entity template, bool isInitializing = false)
        {
            var entity = Entity.CreateInstance(template, isInitializing, OnEntityInitialized);
            lock (_lockObject)
            {
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

        private void OnEntityInitialized(Entity entity)
        {
            lock (_lockObject)
            {
                if (_entitiesToInitialize.Remove(entity)) _entities.Add(entity);
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
