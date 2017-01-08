using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Represents a single light of the scene.
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Intensity[0..1] of Ambient light influence. Set relatively high for brigher scenes.
        /// </summary>
        public float AmbientIntensity { get; set; }

        /// <summary>
        /// Intensity[0..1] of Diffuse light influence.
        /// </summary>
        public float DiffuseIntensity { get; set; }

        /// <summary>
        /// For Light Type Directional. Setting light direction.
        /// </summary>
        public Vector3 Direction { get; set; }

        /// <summary>
        /// For light Type Position. Setting light position in world.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Range of point light in world units.
        /// </summary>
        public float Range { get; set; }

        /// <summary>
        /// Light Type.
        /// </summary>
        public LightType Type { get; set; }

        /// <summary>
        /// LightViewMatrix for rendering shadows properly.
        /// </summary>
        public Matrix LightViewMatrix { get; private set; }

        /// <summary>
        /// Creates a new light instance.
        /// </summary>
        internal Light()
        {
            Type = LightType.Directional;
            AmbientIntensity = 0.1f;
            DiffuseIntensity = 0.9f;
        }

        public RenderTarget2D ShadowMap { get; set; }

        /// <summary>
        /// Calculates the Viewprojection Matrix of light to draw shadow map. This depends on Light Type.
        /// Directional: 
        /// 1) Create a merged bounding box of all shadow casters.
        /// 2) create enclosing sphere of boundingbox from 1) to find out to position light by taking radius as distance to ensure see all shadow casters. 
        /// 3) Transform AABB from 1) to viewspace of light and rebuild an AABB for this. this sets the size for orthogonal projection matrix.
        /// </summary>
        /// <param name="camera">needed for billboards.</param>
        /// <param name="shadowCasters">All Nodes casting a shadow for the scene.</param>
        internal void UpdateLightViewMatrixForCamera(Camera camera, IList<DrawableElement> shadowCasters)
        {
            switch (Type)
            {
                case LightType.Directional:
                    UpdateLightMatrixForDirectionalLight(camera, shadowCasters);
                    break;
                case LightType.Point:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        private void UpdateLightMatrixForDirectionalLight(Camera camera, IList<DrawableElement> shadowCasters)
        {
            var directionNormalized = Vector3.Normalize(Direction);
            var lightViewMatrix = Matrix.CreateLookAt(Vector3.Zero, Direction, Vector3.Up);

            var mergedBox = new BoundingBox();
            for (var i = 0; i < shadowCasters.Count; i++)
            {
                var drawableElement = shadowCasters[i];
                mergedBox = BoundingBox.CreateMerged(mergedBox, drawableElement.BoundingBox);
            }
            var sphere = BoundingSphere.CreateFromBoundingBox(mergedBox);

            var edges = new Vector3[8];
            edges[0] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Min.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[1] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Min.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[2] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Min.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[3] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Min.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[4] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Max.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[5] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Max.Y, mergedBox.Min.Z), lightViewMatrix);
            edges[6] = Vector3.Transform(new Vector3(mergedBox.Min.X, mergedBox.Max.Y, mergedBox.Max.Z), lightViewMatrix);
            edges[7] = Vector3.Transform(new Vector3(mergedBox.Max.X, mergedBox.Max.Y, mergedBox.Max.Z), lightViewMatrix);

            var boundingBox = BoundingBox.CreateFromPoints(edges);
            var width = boundingBox.Max.X - boundingBox.Min.X;
            var height = boundingBox.Max.Y - boundingBox.Min.Y;

            var cameraPosition = sphere.Center - directionNormalized * sphere.Radius;
            var cameraPositionTarget = cameraPosition + directionNormalized;

            LightViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPositionTarget, Vector3.Up) * Matrix.CreateOrthographic(width, height, 0.1f, sphere.Radius * 2);
        }
    }
}
