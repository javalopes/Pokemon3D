using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps.EntityComponents;
using Pokemon3D.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Pokemon3D.GameModes.Maps
{
    /// <summary>
    /// Represents a functional part of a map.
    /// </summary>
    class Entity : GameObject
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

        public Entity Parent { get; private set; }
        public ReadOnlyCollection<Entity> Children { get; private set; }
        public bool IsStatic { get; set; }

        public Entity()
        {
            _isActive = true;
            _childNodes = new List<Entity>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            Right = Vector3.Right;
            Up = Vector3.Up;
            Forward = Vector3.Forward;
            SetDirty();
        }

        /// <summary>
        /// Entity update method to update all of the entity's components.
        /// </summary>
        public void Update(float elapsedTime)
        {
            HandleIsDirty();
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].IsActive) _components[i].Update(elapsedTime);
            }
        }

        public void RenderPreparations(Camera observer)
        {
            for (int i = 0; i < _components.Count; i++) _components[i].RenderPreparations(observer);
        }

        public T AddComponent<T>(T component) where T : EntityComponent
        {
            if (!HasComponent(component.Name))
            {
                _components.Add(component);
                component.OnComponentAdded();
            }
            return component;
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
                        _components[i].OnIsActiveChanged();
                    }
                    for (var i = 0; i < _childNodes.Count; i++)
                    {
                        _childNodes[i].IsActive = _isActive;
                    }
                }

            }
        }

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

        public Vector3 GlobalEulerAngles
        {
            get
            {
                HandleIsDirty();
                return _globalEulerAngles;
            }
        }

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

        public Vector3 GlobalPosition
        {
            get
            {
                HandleIsDirty();
                return _globalPosition;
            }
            private set { _globalPosition = value; }
        }

        public Vector3 Right
        {
            get
            {
                HandleIsDirty();
                return _right;
            }
            private set { _right = value; }
        }

        public Vector3 Up
        {
            get
            {
                HandleIsDirty();
                return _up;
            }
            private set { _up = value; }
        }

        public Vector3 Forward
        {
            get
            {
                HandleIsDirty();
                return _forward;
            }
            private set { _forward = value; }
        }

        public void SetParent(Entity parent)
        {
            if (parent == Parent) return;
            Parent?.RemoveChild(this);
            parent?.AddChild(this);
            SetDirty();
        }

        public void AddChild(Entity childElement)
        {
            childElement.Parent?.RemoveChild(childElement);
            _childNodes.Add(childElement);
            childElement.Parent = this;
        }

        public void RemoveChild(Entity childElement)
        {
            if (_childNodes.Remove(childElement))
            {
                childElement.Parent = null;
            }
        }

        public void Translate(Vector3 translation)
        {
            Position += Right * translation.X + Up * translation.Y + Forward * translation.Z;
            SetDirty();
        }

        public void RotateX(float angle)
        {
            EulerAngles += new Vector3(angle, 0, 0);
            SetDirty();
        }

        public void RotateY(float angle)
        {
            EulerAngles += new Vector3(0, angle, 0);
            SetDirty();
        }

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
            WorldMatrix = Parent == null ? localWorldMatrix : localWorldMatrix * Parent.WorldMatrix;

            GlobalPosition = Parent != null ? new Vector3(WorldMatrix.M41, WorldMatrix.M42, WorldMatrix.M43) : _position;

            var rotationMatrix = Matrix.CreateFromYawPitchRoll(_globalEulerAngles.Y, _globalEulerAngles.X, _globalEulerAngles.Z);
            _right = Vector3.TransformNormal(Vector3.Right, rotationMatrix);
            _up = Vector3.TransformNormal(Vector3.Up, rotationMatrix);
            _forward = Vector3.TransformNormal(Vector3.Forward, rotationMatrix);

            _isDirty = false;
        }

        public Matrix WorldMatrix { get; private set; }

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

        public T GetComponent<T>() where T : EntityComponent
        {
            return _components.FirstOrDefault(c => c is T) as T;
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
            return _components.Any(c => (c.Name ?? "").Equals(componentName, System.StringComparison.OrdinalIgnoreCase)
                                        && c is T);
        }

        public bool HasComponent<T>() where T : EntityComponent
        {
            return _components.Any(c => c is T);
        }
    }
}
