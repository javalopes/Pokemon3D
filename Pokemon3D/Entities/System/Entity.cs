using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity
    {
        private readonly List<EntityComponent> _components = new List<EntityComponent>();
        private readonly List<Entity> _childNodes;

        private Vector3 _rotationAxis;
        private Vector3 _position;
        private Vector3 _scale;
        private bool _isDirty;
        private Vector3 _globalPosition;
        private Vector3 _globalEulerAngles;
        private Vector3 _right;
        private Vector3 _up;
        private Vector3 _forward;
        private bool _isActive;
        private readonly Action<Entity> _onIsInitialized;
        private Matrix _worldMatrix;

        /// <summary>
        /// Parent Entity. Inherits transformation.
        /// </summary>
        public Entity Parent { get; private set; }

        /// <summary>
        /// All child entities which inherits transformation from this entity.
        /// </summary>
        public ReadOnlyCollection<Entity> Children { get; private set; }

        /// <summary>
        /// This entity is not able to move after creation.
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Last Transformation offset since update.
        /// </summary>
        public Vector3 LastTranslation { get; private set; }

        /// <summary>
        /// Identification of entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// If the entity is created in initializing mode.
        /// When creating an entity from a background thread this should be true until the whole entity is loaded.
        /// </summary>
        public bool IsInitializing { get; private set; }

        /// <summary>
        /// Creates an new entity. Should only be called internally by the <see cref="EntitySystem"/>
        /// </summary>
        /// <param name="isInitializing">If this entity is created in the initializing mode.</param>
        /// <param name="onIsInitialized">Action called when entity is initialized.</param>
        public Entity(bool isInitializing = false, Action<Entity> onIsInitialized = null)
        {
            _isActive = true;
            _childNodes = new List<Entity>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = Vector3.Forward;
            SetDirty();
            LastTranslation = Vector3.Zero;
            IsInitializing = isInitializing;
            _onIsInitialized = onIsInitialized;
        }

        /// <summary>
        /// Entity update method to update all of the entity's components.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            HandleIsDirty();
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].IsActive) _components[i].Update(gameTime);
            }
            LastTranslation = Vector3.Zero;
        }

        public void EndInitializing(bool suppressInvoke = false)
        {
            if (!IsInitializing) return;
            IsInitializing = true;

            foreach (var component in _components)
            {
                component.OnInitialized();
            }

            if (!suppressInvoke) _onIsInitialized?.Invoke(this);
        }
        
        /// <summary>
        /// Adds an component to the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public T AddComponent<T>(T component) where T : EntityComponent
        {
            if (!HasComponent(component.Name))
            {
                _components.Add(component);
                component.OnComponentAdded();
            }
            return component;
        }

        /// <summary>
        /// Removes a component of the entity by name.
        /// </summary>
        /// <param name="componentName"></param>
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
        /// Allows to activate or deactivate entity. This is also notified to children.
        /// </summary>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    for (var i = 0; i < _components.Count; i++)
                    {
                        _components[i].IsActive = _isActive;
                    }
                    for (var i = 0; i < _childNodes.Count; i++)
                    {
                        _childNodes[i].IsActive = _isActive;
                    }
                }

            }
        }

        /// <summary>
        /// Gets or sets the scale of the entity.
        /// </summary>
        public Vector3 Scale
        {
            get
            {
                HandleIsDirty();
                return _scale;
            }
            set
            {
                _scale = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Gets or sets the roation angles of the entity.
        /// </summary>
        public Vector3 EulerAngles
        {
            get
            {
                HandleIsDirty();
                return _rotationAxis;
            }
            set
            {
                _rotationAxis = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Computed global euler angles derived from parent.
        /// </summary>
        public Vector3 GlobalEulerAngles
        {
            get
            {
                HandleIsDirty();
                return _globalEulerAngles;
            }
        }

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        public Vector3 Position
        {
            get
            {
                HandleIsDirty();
                return _position;
            }
            set
            {
                _position = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Computed global position derived from parent.
        /// </summary>
        public Vector3 GlobalPosition
        {
            get
            {
                HandleIsDirty();
                return _globalPosition;
            }
            private set { _globalPosition = value; }
        }

        /// <summary>
        /// Unit-Vector pointing to right of entity regarding to rotation.
        /// </summary>
        public Vector3 Right
        {
            get
            {
                HandleIsDirty();
                return _right;
            }
            private set { _right = value; }
        }

        /// <summary>
        /// Unit-Vector pointing to up of entity regarding to rotation.
        /// </summary>
        public Vector3 Up
        {
            get
            {
                HandleIsDirty();
                return _up;
            }
            private set { _up = value; }
        }

        /// <summary>
        /// Unit-Vector pointing forward of entity regarding to rotation.
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                HandleIsDirty();
                return _forward;
            }
            private set { _forward = value; }
        }

        /// <summary>
        /// Sets the parent of this entity. old entity will remove it.
        /// </summary>
        /// <param name="parent">Parent. Can be null to unset parent.</param>
        public void SetParent(Entity parent)
        {
            if (parent == Parent) return;
            Parent?.RemoveChild(this);
            parent?.AddChild(this);
            SetDirty();
        }

        /// <summary>
        /// Adds a child entity to this entity.
        /// </summary>
        /// <param name="childElement">Child to add.</param>
        public void AddChild(Entity childElement)
        {
            if (childElement == null) throw new ArgumentNullException(nameof(childElement));

            childElement.Parent?.RemoveChild(childElement);
            _childNodes.Add(childElement);
            childElement.Parent = this;
        }

        /// <summary>
        /// Removes a child from the entity.
        /// </summary>
        /// <param name="childElement"></param>
        public void RemoveChild(Entity childElement)
        {
            if (_childNodes.Remove(childElement))
            {
                childElement.Parent = null;
            }
        }

        /// <summary>
        /// Translate the entity with rotation aware around XYZ axis.
        /// </summary>
        /// <param name="translation"></param>
        public void Translate(Vector3 translation)
        {
            LastTranslation = translation;
            Position += Right * translation.X + Up * translation.Y + Forward * translation.Z;
            SetDirty();
        }

        /// <summary>
        /// Rotates around <see cref="Right"/>
        /// </summary>
        /// <param name="angle"></param>
        public void RotateX(float angle)
        {
            EulerAngles += new Vector3(angle, 0, 0);
            SetDirty();
        }

        /// <summary>
        /// Rotates around <see cref="Up"/>
        /// </summary>
        /// <param name="angle"></param>
        public void RotateY(float angle)
        {
            EulerAngles += new Vector3(0, angle, 0);
            SetDirty();
        }

        /// <summary>
        /// Rotates around <see cref="Forward"/>
        /// </summary>
        /// <param name="angle"></param>
        public void RotateZ(float angle)
        {
            EulerAngles += new Vector3(0, 0, angle);
            SetDirty();
        }

        protected void SetDirty()
        {
            _isDirty = true;
            for (var i = 0; i < _childNodes.Count; i++)
            {
                _childNodes[i].SetDirty();
            }
        }

        private void HandleIsDirty()
        {
            if (!_isDirty) return;

            _globalEulerAngles = Parent != null ? Parent.GlobalEulerAngles + _rotationAxis : _rotationAxis;

            var localWorldMatrix = Matrix.CreateScale(_scale) * Matrix.CreateFromYawPitchRoll(_rotationAxis.Y, _rotationAxis.X, _rotationAxis.Z) *
                                   Matrix.CreateTranslation(_position);

            Parent?.HandleIsDirty();
            _worldMatrix = Parent == null ? localWorldMatrix : localWorldMatrix * Parent.WorldMatrix;

            GlobalPosition = Parent != null ? new Vector3(_worldMatrix.M41, _worldMatrix.M42, _worldMatrix.M43) : _position;

            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_globalEulerAngles.Y, _globalEulerAngles.X, _globalEulerAngles.Z);
            _right = Vector3.TransformNormal(Vector3.Right, rotationMatrix);
            _up = Vector3.TransformNormal(Vector3.Up, rotationMatrix);
            _forward = Vector3.TransformNormal(Vector3.Forward, rotationMatrix);

            _isDirty = false;
        }

        public void OnRemoved()
        {
            for (var i = 0; i < _components.Count; i++)
            {
                _components[i].OnComponentRemove();
            }
            _components.Clear();
        }

        /// <summary>
        /// Entity world matrix.
        /// </summary>
        public Matrix WorldMatrix
        {
            get
            {
                HandleIsDirty();
                return _worldMatrix;
            }
            private set { _worldMatrix = value; }
        }

        /// <summary>
        /// Returns a component of this <see cref="Entity"/>.
        /// </summary>
        public EntityComponent GetComponent(string componentName)
        {
            return _components.FirstOrDefault(c => c.Name.Equals(componentName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns a component of a specific type of this <see cref="Entity"/>.
        /// </summary>
        public T GetComponent<T>(string componentName) where T : EntityComponent
        {
            return GetComponent(componentName) as T;
        }

        public T GetComponent<T>() where T : EntityComponent
        {
            return _components.FirstOrDefault(c => c is T) as T;
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific name.
        /// </summary>
        public bool HasComponent(string componentName)
        {
            return _components.Any(c => (c.Name ?? "").Equals(componentName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns if this <see cref="Entity"/> has a component of a specific type and name.
        /// </summary>
        public bool HasComponent<T>(string componentName) where T : EntityComponent
        {
            return _components.Any(c => (c.Name ?? "").Equals(componentName, StringComparison.OrdinalIgnoreCase)
                                        && c is T);
        }

        public bool HasComponent<T>() where T : EntityComponent
        {
            return _components.Any(c => c is T);
        }

        public int ComponentCount => _components.Count;
    }
}
