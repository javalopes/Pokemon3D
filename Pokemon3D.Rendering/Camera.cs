﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Specialized Scene Node representing a camera.
    /// </summary>
    public class Camera
    {
        public float NearClipDistance { get; set; }
        public float FarClipDistance { get; set; }
        public float FieldOfView { get; set; }
        public Viewport Viewport { get; set; }

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        public Vector3 GlobalEulerAngles { get; set; }
        public Vector3 GlobalPosition { get; set; }
        public BoundingFrustum Frustum { get; }

        public Color? ClearColor { get; set; }
        public Skybox Skybox { get; set; }
        
        internal Camera(Viewport viewport)
        {
            Viewport = viewport;
            NearClipDistance = 0.1f;
            FarClipDistance = 1000.0f;
            FieldOfView = MathHelper.PiOver4;
            Frustum = new BoundingFrustum(Matrix.Identity);
            ClearColor = Color.CornflowerBlue;
        }

        public void Update(Matrix world)
        {
            ViewMatrix = Matrix.Invert(world);
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, Viewport.AspectRatio, NearClipDistance, FarClipDistance);
            Frustum.Matrix = ViewMatrix * ProjectionMatrix;
        }

        internal void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize)
        {
            Viewport = new Viewport(0,0,newSize.Width, newSize.Height);
        }
    }
}
