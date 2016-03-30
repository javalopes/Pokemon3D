using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
using System;
// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Part of a Scene with contains Transformation and Optional Rendering Attachments.
    /// SceneNodes can be arranged in a hierarchy to allow complex transformations.
    /// </summary>
    public class DrawableElement : IdObject
    {
        private Action<DrawableElement> _onEndInitializing;

        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        public Matrix WorldMatrix { get; set; }
        public bool IsActive { get; set; }
        public bool IsBillboard { get; set; }

        public bool IsInitializing { get; private set; }

        public DrawableElement(bool initializing = false, Action<DrawableElement> onEndInitializing = null)
        {
            IsInitializing = initializing;
            if (initializing) _onEndInitializing = onEndInitializing;
            IsActive = true;
        }

        public void EndInitialzing()
        {
            if (!IsInitializing) throw new ApplicationException("DrawableElement is not initializing");
            _onEndInitializing(this);
            _onEndInitializing = null;
            IsInitializing = false;
        }

        public Vector3 GlobalPosition
        {
            get; set;
        }

        public BoundingBox BoundingBox
        {
            get; set;
        }
    }
}
