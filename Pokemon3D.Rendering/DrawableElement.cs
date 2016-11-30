﻿using Microsoft.Xna.Framework;
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

        public int CameraMask { get; }
        public Mesh Mesh { get; set; }
        public Material Material { get; set; }
        public Matrix WorldMatrix { get; set; }
        public bool IsActive { get; set; }
        public bool IsBillboard { get; set; }
        public Vector3 GlobalPosition { get; set; }
        public Vector3 Scale { get; set; }
        public BoundingBox BoundingBox { get; set; }
        
        public bool IsInitializing { get; private set; }

        public DrawableElement(int cameraMask = 1, bool initializing = false, Action<DrawableElement> onEndInitializing = null)
        {
            CameraMask = cameraMask;
            IsInitializing = initializing;
            _onEndInitializing = onEndInitializing;
            IsActive = true;
            Scale = Vector3.One;
        }

        public void UpdateBounds()
        {
            var bounds = Mesh.LocalBounds;
            var point1 = Vector3.Transform(bounds.Min, WorldMatrix);
            var point2 = Vector3.Transform(bounds.Max, WorldMatrix);
            BoundingBox = BoundingBox.CreateFromPoints(new[] { point1, point2 });
        }

        public void EndInitialzing()
        {
            if (!IsInitializing) throw new ApplicationException("DrawableElement is not initializing");
            _onEndInitializing(this);
            _onEndInitializing = null;
            IsInitializing = false;
        }

        public Matrix GetWorldMatrix(float cameraRotationY)
        {
            if (IsBillboard) return Matrix.CreateScale(Scale) * Matrix.CreateRotationY(cameraRotationY) * Matrix.CreateTranslation(GlobalPosition);
            return WorldMatrix;
        }
    }
}
