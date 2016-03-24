using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Part of a Scene with contains Transformation and Optional Rendering Attachments.
    /// SceneNodes can be arranged in a hierarchy to allow complex transformations.
    /// </summary>
    public class SceneNode : IdObject, DrawableElement
    {
        private readonly List<SceneNode> _childNodes;
        private Action<SceneNode> _onInitializationFinished;

        public SceneNode Parent { get; private set; }
        public ReadOnlyCollection<SceneNode> Children { get; private set; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        public Matrix WorldMatrix { get; set; }

        private Vector3 _rotationAxis;
        private Vector3 _position;
        private Vector3 _scale;
        private bool _isDirty;
        private Matrix _world;
        private Vector3 _globalPosition;
        private Vector3 _globalEulerAngles;
        private Vector3 _right;
        private Vector3 _up;
        private Vector3 _forward;
        private bool _isActive;

        internal SceneNode(bool isInitializing, Action<SceneNode> onInitializationFinished)
        {
            _onInitializationFinished = onInitializationFinished;
            _isActive = true;
            _childNodes = new List<SceneNode>();
            Children = _childNodes.AsReadOnly();
            _scale = Vector3.One;
            IsInitializing = isInitializing;
        }

        public bool IsInitializing { get; private set; }

        public void EndInitializing()
        {
            if (!IsInitializing) throw new InvalidOperationException("Scene Node is already initialized");
            IsInitializing = false;
            _onInitializationFinished(this);
        }
        
        public bool IsActive
        {
            get; set;
        }

        public bool IsBillboard { get; set; }

        public Vector3 GlobalPosition
        {
            get; set;
        }

        public BoundingBox BoundingBox
        {
            get; set;
        }

        internal SceneNode Clone(bool cloneMesh)
        {
            var sceneNode = new SceneNode(false, null)
            {
                Mesh = cloneMesh ? Mesh?.Clone() : Mesh,
                Material = Material.Clone(),
            };
            return sceneNode;
        }
    }
}
