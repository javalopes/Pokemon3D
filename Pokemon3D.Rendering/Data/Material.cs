﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Describing how to display a Mesh attached to a SceneNode.
    /// </summary>
    public class Material
    {
        public Material(Texture2D diffuseTexture)
        {
            DiffuseTexture = diffuseTexture;
            Color = Color.White;
            CastShadow = true;
            ReceiveShadow = true;
            TexcoordOffset = Vector2.Zero;
            TexcoordScale = Vector2.One;
        }

        public bool UseTransparency { get; set; }
        public Texture2D DiffuseTexture { get; set; }
        public Color Color { get; set; }
        public bool CastShadow { get; set; }
        public bool ReceiveShadow { get; set; }
        public Vector2 TexcoordScale { get; set; }
        public Vector2 TexcoordOffset { get; set; }

        internal Material Clone()
        {
            return new Material(DiffuseTexture)
            {
                Color = Color,
                UseTransparency = UseTransparency,
                CastShadow = CastShadow,
                ReceiveShadow = ReceiveShadow,
                TexcoordOffset = TexcoordOffset,
                TexcoordScale = TexcoordScale
            };
        }
    }
}