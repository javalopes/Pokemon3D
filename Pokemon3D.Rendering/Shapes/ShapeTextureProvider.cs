using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Shapes
{
    /// <summary>
    /// Creates <see cref="Texture2D"/> for a <see cref="Shape"/>.
    /// </summary>
    public interface ShapeTextureProvider
    {
        Texture2D GetTexture(Shape shape, object[] textureData);
    }
}
