using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Shapes
{
    public class SingleColorShapeTextureProvider : ShapeTextureProvider
    {
        private readonly ShapeRenderer _renderer;
        private readonly Dictionary<int, Texture2D> _buffer = new Dictionary<int, Texture2D>();

        public SingleColorShapeTextureProvider(ShapeRenderer renderer)
        {
            _renderer = renderer;
        }

        private Texture2D CreateTexture(Shape shape)
        {
            // ignore any texture data.

            var bounds = shape.Bounds;
            Color[] colorArr = new Color[bounds.Width * bounds.Height];

            for (int x = 0; x < bounds.Width; x++)
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    var index = y * bounds.Width + x;

                    if (shape.Contains(x + bounds.X, y + bounds.Y))
                        colorArr[index] = Color.White;
                    else
                        colorArr[index] = Color.Transparent;
                }
            }

            var texture = new Texture2D(_renderer.GraphicsDevice, bounds.Width, bounds.Height);
            texture.SetData(colorArr);
            return texture;
        }

        public Texture2D GetTexture(Shape shape, object[] textureData)
        {
            var hash = shape.GetHashCode();
            Texture2D texture;
            if (_buffer.TryGetValue(hash, out texture)) return texture;

            texture = CreateTexture(shape);
            _buffer.Add(hash, texture);

            return texture;
        }
    }
}
