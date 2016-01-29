using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;

namespace Pokemon3D.Rendering.Data
{
    /// <summary>
    /// Describing how to display a Mesh attached to a SceneNode.
    /// </summary>
    public class Material
    {
        public Material()
        {
            DiffuseTexture = null;
            Color = Color.White;
            CastShadow = true;
            ReceiveShadow = true;
            TexcoordOffset = Vector2.Zero;
            TexcoordScale = Vector2.One;
            IsUnlit = false;
        }

        public bool UseTransparency { get; set; }

        public Texture2D DiffuseTexture { get; set; }

        public Color Color { get; set; }

        public bool CastShadow { get; set; }

        public bool ReceiveShadow { get; set; }

        public Vector2 TexcoordScale { get; set; }

        public Vector2 TexcoordOffset { get; set; }

        public bool IsUnlit { get; set; }

        public string CompareId =>
            $"{((DiffuseTexture?.Name) ?? "null").GetHashCode()}_{(UseTransparency ? "1" : "0")}_{(ReceiveShadow ? "1" : "0")}_{(IsUnlit ? "1" : "0")}";

        internal Material Clone()
        {
            return new Material
            {
                Color = Color,
                DiffuseTexture = DiffuseTexture,
                UseTransparency = UseTransparency,
                CastShadow = CastShadow,
                ReceiveShadow = ReceiveShadow,
                TexcoordOffset = TexcoordOffset,
                TexcoordScale = TexcoordScale,
                IsUnlit = IsUnlit
            };
        }

        internal int GetLightingTypeFlags(RenderSettings renderSettings)
        {
            int flags = 0;

            if (!IsUnlit) flags |= LightTechniqueFlag.Lit;
            if (ReceiveShadow && renderSettings.EnableShadows) flags |= LightTechniqueFlag.ReceiveShadows;
            if (renderSettings.EnableSoftShadows) flags |= LightTechniqueFlag.SoftShadows;
            if (DiffuseTexture != null) flags |= LightTechniqueFlag.UseTexture;

            return flags;
        }
    }
}
