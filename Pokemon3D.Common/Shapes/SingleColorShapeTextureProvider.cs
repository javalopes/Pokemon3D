using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common.Shapes
{
    class SingleColorShapeTextureProvider : ShapeTextureProvider
    {
        public SingleColorShapeTextureProvider(ShapeRenderer renderer) 
            : base(renderer)
        {  }

        protected override Texture2D CreateTexture(Shape shape)
        {
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
    }
}
