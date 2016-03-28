using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Pokemon3D.Common.Shapes
{
    public class SingleColorShapeTextureProvider : ShapeTextureProvider
    {
        private ShapeRenderer _renderer;
        private Dictionary<int, Texture2D> _buffer = new Dictionary<int, Texture2D>();

        public SingleColorShapeTextureProvider(ShapeRenderer renderer)
        {
            _renderer = renderer;
        }

        private Texture2D CreateTexture(Shape shape, object[] textureData)
        {
            // ignore any texture data.

            var bounds = shape.Bounds;
            Color[] colorArr = new Color[bounds.Width * bounds.Height];
            int index = 0;

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    index = y * bounds.Width + x;

                    if (shape.Contains(x + bounds.X, y + bounds.Y))
                        colorArr[index] = Color.White;
                    else
                        colorArr[index] = Color.Transparent;
                }
            }

            Texture2D texture = new Texture2D(_renderer.GraphicsDevice, bounds.Width, bounds.Height);
            texture.SetData(colorArr);
            return texture;
        }

        public Texture2D GetTexture(Shape shape, object[] textureData)
        {
            int hash = shape.GetHashCode();
            Texture2D texture;

            if (!_buffer.TryGetValue(hash, out texture))
            {
                texture = CreateTexture(shape, textureData);
                _buffer.Add(hash, texture);
            }

            return texture;
        }
    }
}
