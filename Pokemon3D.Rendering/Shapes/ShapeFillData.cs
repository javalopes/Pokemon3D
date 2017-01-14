using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Shapes
{
    public struct ShapeFillData
    {
        public ShapeTextureProvider TextureProvider { get; }
        public object[] TextureData { get; }
        public Shape Shape { get; }

        public ShapeFillData(Shape shape, ShapeTextureProvider textureProvider, object[] textureData)
        {
            TextureProvider = textureProvider;
            TextureData = textureData;
            Shape = shape;
        }

        public Texture2D GetTexture()
        {
            return TextureProvider.GetTexture(Shape, TextureData);
        }
    }
}
