using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering
{
    /// <summary>
    /// Holds data describing part of texture.
    /// </summary>
    public struct TextureRegion
    {
        public Texture2D Texture;
        public Rectangle? Rectangle;
    }
}
