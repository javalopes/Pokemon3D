using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common.Shapes
{
    public struct ShapeFillData
    {
        public ShapeTextureProvider TextureProvider { get; private set; }
        public object[] TextureData { get; private set; }
        public Shape Shape { get; private set; }

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
