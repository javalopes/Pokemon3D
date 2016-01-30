using Microsoft.Xna.Framework;
using Pokemon3D.Common.Extensions;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameMode.Map.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Collisions;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;

namespace Pokemon3D.GameModes.Maps
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity : GameObject
    {
        public Scene Scene { get; }
        public SceneNode SceneNode { get; }
        public RenderMethod RenderMethod => _dataModel.RenderMode.RenderMethod;

        public TextureSourceModel[] TextureSources => _dataModel.RenderMode.Textures;

        private readonly EntityModel _dataModel;
        private readonly EntityFieldPositionModel _fieldSourceModel;
        private readonly Dictionary<string, EntityComponent> _components = new Dictionary<string, EntityComponent>();
        private readonly Map _map;
        private Collider _collider;

        public Entity(Map map, EntityModel dataModel, EntityFieldPositionModel fieldSourceModel, Vector3 position) :
            this(map.Scene)
        {
            _map = map;
            _dataModel = dataModel;
            _fieldSourceModel = fieldSourceModel;

            InitializeComponents();

            SceneNode.Scale = _fieldSourceModel.Scale?.GetVector3() ?? Vector3.One;

            if (_fieldSourceModel.Rotation != null)
            {
                if (_fieldSourceModel.CardinalRotation)
                {
                    SceneNode.EulerAngles = new Vector3
                    {
                        X = _fieldSourceModel.Rotation.X * MathHelper.PiOver2,
                        Y = _fieldSourceModel.Rotation.Y * MathHelper.PiOver2,
                        Z = _fieldSourceModel.Rotation.Z * MathHelper.PiOver2
                    };
                }
                else
                {
                    SceneNode.EulerAngles = new Vector3
                    {
                        X = MathHelper.ToDegrees(_fieldSourceModel.Rotation.X),
                        Y = MathHelper.ToDegrees(_fieldSourceModel.Rotation.Y),
                        Z = MathHelper.ToDegrees(_fieldSourceModel.Rotation.Z)
                    };
                }
            }
            else
            {
                SceneNode.EulerAngles = Vector3.Zero;
            }

            SceneNode.Position = position;
            SceneNode.Material = new Material();

            var renderMode = _dataModel.RenderMode;

            if (renderMode != null)
            {
                if (renderMode.RenderMethod == RenderMethod.Primitive)
                {
                    SceneNode.Mesh = map.GameMode.GetPrimitiveMesh(renderMode.PrimitiveModelId);
                    SceneNode.Material.Color = new Color(_dataModel.RenderMode.Shading.GetVector3());
                    SceneNode.Material.CastShadow = false;
                    SceneNode.Material.ReceiveShadow = !_dataModel.RenderMode.UseTransparency;
                    SceneNode.Material.UseTransparency = _dataModel.RenderMode.UseTransparency;
                    SceneNode.Material.IsUnlit = false;

                    SetTexture(0);
                }
                else
                {
                    var models = _map.GameMode.GetModel(renderMode.ModelPath);

                    if (models.Length == 1)
                    {
                        AttachModelToSceneNode(SceneNode, models[0]);
                    }
                    else
                    {
                        foreach(var modelMesh in models)
                        {
                            var childNode = Scene.CreateSceneNode(true);
                            AttachModelToSceneNode(childNode, modelMesh);
                            SceneNode.AddChild(childNode);
                            childNode.EndInitializing();
                        }
                    }
                }
            }
            
            if (IsStatic)
            {
                Scene.ConvertToStaticSceneNode(SceneNode);
            }

            SceneNode.EndInitializing();

            var colliderComponent = GetComponent<CollisionEntityComponent>(EntityComponent.IDs.Collision);
            if (colliderComponent != null)
            {
                Collider = Collider.CreateBoundingBox(colliderComponent.GetCollisionSizeVector3(), colliderComponent.GetCollisionOffset());
                Collider.SetPosition(Position);
            }
        }

        private void AttachModelToSceneNode(SceneNode sceneNode, ModelMesh modelMesh)
        {
            sceneNode.Mesh = modelMesh.Mesh;
            sceneNode.Material = modelMesh.Material;
            sceneNode.Material.Color = new Color(_dataModel.RenderMode.Shading.GetVector3());
            sceneNode.Material.CastShadow = true;
            sceneNode.Material.ReceiveShadow = !_dataModel.RenderMode.UseTransparency;
            sceneNode.Material.UseTransparency = _dataModel.RenderMode.UseTransparency;
            sceneNode.Material.IsUnlit = false;
        }

        public Entity(Scene scene)
        {
            Scene = scene;
            SceneNode = scene.CreateSceneNode(true);
        }

        /// <summary>
        /// The absolute position of this entity in the world.
        /// </summary>
        public Vector3 Position
        {
            get { return SceneNode.Position; }
            set
            {
                if (!IsStatic)
                    SceneNode.Position = value;
            }
        }

        /// <summary>
        /// The scale of this entity, relative to <see cref="Vector3.One"/>.
        /// </summary>
        public Vector3 Scale
        {
            get { return SceneNode.Scale; }
            set
            {
                if (!IsStatic)
                    SceneNode.Scale = value;
            }
        }

        /// <summary>
        /// Returns, if this entity is marked as static. When an entity is static, it cannot modify its position, rotation and scale.
        /// </summary>
        public bool IsStatic
        {
            get { return HasComponent(EntityComponent.IDs.Static); }
            set
            {
                if (value && !IsStatic)
                {
                    AddComponent(EntityComponentFactory.Instance.GetComponent(this, EntityComponent.IDs.Static));
                }
                else if (!value && IsStatic)
                {
                    RemoveComponent(EntityComponent.IDs.Static);
                }
            }
        }

        /// <summary>
        /// Sets the texture of the entity to the texture at the provided index in the data model.
        /// </summary>
        public void SetTexture(int index)
        {
            var texture = _dataModel.RenderMode.Textures[index];

            var diffuseTexture = _map.GameMode.GetTexture(texture.Source);
            SceneNode.Material.DiffuseTexture = diffuseTexture;

            if (texture.Rectangle != null)
            {
                SceneNode.Material.TexcoordOffset = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.X, texture.Rectangle.Y);
                SceneNode.Material.TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(texture.Rectangle.Width, texture.Rectangle.Height);
            }
            else
            {
                SceneNode.Material.TexcoordOffset = Vector2.Zero;
                SceneNode.Material.TexcoordScale = Vector2.One;
            }
        }

        /// <summary>
        /// Entity update method to update all of the entity's components.
        /// </summary>
        public virtual void Update(float elapsedTime)
        {
            for (int i = 0; i < _components.Count; i++)
                _components.Values.ElementAt(i).Update(elapsedTime);
        }

        public void RenderPreparations(Camera observer)
        {
            for (int i = 0; i < _components.Count; i++)
                _components.Values.ElementAt(i).RenderPreparations(observer);
        }

        #region Components

        void InitializeComponents()
        {
            // Loops over the data models of the entity components and creates actual instances.

            var factory = EntityComponentFactory.Instance;

            foreach (var compModel in _dataModel.Components)
            {
                if (!HasComponent(compModel.Id))
                {
                    var comp = factory.GetComponent(this, compModel);
                    AddComponent(comp);
                }
            }
        }

        public void AddComponent(EntityComponent component)
        {
            if (!HasComponent(component.Name))
            {
                _components.Add(component.Name.ToLowerInvariant(), component);
                component.OnComponentAdded();
            }
        }

        public void RemoveComponent(string componentName)
        {
            if (HasComponent(componentName))
            {
                _components[componentName.ToLowerInvariant()].OnComponentRemove();
                _components.Remove(componentName.ToLowerInvariant());
            }
        }

        /// <summary>
        /// Returns a component of this <see cref="Entity"/>.
        /// </summary>
        public EntityComponent GetComponent(string componentName)
        {
            if (HasComponent(componentName))
                return _components[componentName.ToLowerInvariant()];

            return null;
        }

        /// <summary>
        /// Returns a component of a specific type of this <see cref="Entity"/>.
        /// </summary>
        public T GetComponent<T>(string componentName) where T : EntityComponent
        {
            if (HasComponent<T>(componentName))
                return (T)_components[componentName.ToLowerInvariant()];

            return null;
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific name.
        /// </summary>
        public bool HasComponent(string componentName)
        {
            return _components.Keys.Contains(componentName.ToLowerInvariant());
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

        public Collider Collider
        {
            get { return _collider; }
            set
            {
                if (_collider == value) return;

                if (_collider != null) Game.CollisionManager.RemoveCollider(_collider);
                _collider = value;
                if (_collider != null) Game.CollisionManager.AddCollider(_collider);
            }
        }
    }
}
