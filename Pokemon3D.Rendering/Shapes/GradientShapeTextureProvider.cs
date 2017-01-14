using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.Shapes
{
    class GradientShapeTextureProvider : ShapeTextureProvider
    {
        private const string HashPattern = "{0}:{1}:{2}:{3}";

        private readonly Dictionary<string, Texture2D> _buffer = new Dictionary<string, Texture2D>();
        private readonly ShapeRenderer _renderer;

        public GradientShapeTextureProvider(ShapeRenderer renderer)
        {
            _renderer = renderer;
        }

        private Texture2D CreateTexture(Shape shape, Color fromColor, Color toColor, bool vertical)
        {
            var bounds = shape.Bounds;
            Color[] colorArr = new Color[bounds.Width * bounds.Height];
            int index;
            float step;

            int diffR = toColor.R - fromColor.R;
            int diffG = toColor.G - fromColor.G;
            int diffB = toColor.B - fromColor.B;

            if (vertical)
            {
                for (int x = 0; x < bounds.Width; x++)
                {
                    step = x / (float)bounds.Width;
                    Color stepColor = new Color((int)(fromColor.R + diffR * step), (int)(fromColor.G + diffG * step), (int)(fromColor.B + diffB * step));

                    for (int y = 0; y < bounds.Height; y++)
                    {
                        index = y * bounds.Width + x;
                        if (shape.Contains(x + bounds.X, y + bounds.Y))
                        {
                            colorArr[index] = stepColor;
                        }
                        else
                            colorArr[index] = Color.Transparent;
                    }
                }
            }
            else // horizontal
            {
                for (int y = 0; y < bounds.Height; y++)
                {
                    step = y / (float)bounds.Height;
                    Color stepColor = new Color((int)(fromColor.R + diffR * step), (int)(fromColor.G + diffG * step), (int)(fromColor.B + diffB * step));

                    for (int x = 0; x < bounds.Width; x++)
                    {
                        index = y * bounds.Width + x;
                        if (shape.Contains(x + bounds.X, y + bounds.Y))
                        {
                            colorArr[index] = stepColor;
                        }
                        else
                            colorArr[index] = Color.Transparent;
                    }
                }
            }
            
            Texture2D texture = new Texture2D(_renderer.GraphicsDevice, bounds.Width, bounds.Height);
            texture.SetData(colorArr);
            return texture;
        }

        public Texture2D GetTexture(Shape shape, object[] textureData)
        {
            var fromColor = (Color)textureData[0];
            var toColor = (Color)textureData[1];
            var vertical = (bool)textureData[2];

            string hash = string.Format(HashPattern, shape.GetHashCode().ToString(), fromColor.ToString(), toColor.ToString(), vertical.ToString());
            Texture2D texture;

            if (!_buffer.TryGetValue(hash, out texture))
            {
                texture = CreateTexture(shape, fromColor, toColor, vertical);
                _buffer.Add(hash, texture);
            }

            return texture;
        }
    }
}
