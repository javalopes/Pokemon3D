using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using System;

namespace Pokemon3D.Common.Shapes
{
    public class ShapeRenderer
    {
        private readonly Texture2D _canvas;
        private readonly SpriteBatch _batch;
        private SingleColorShapeTextureProvider _singleColorTextureProvider;

        public ShapeRenderer(SpriteBatch spriteBatch)
        {
            _batch = spriteBatch;
            // create a 1x1 white texture to use as canvas for rectangle and line shapes.
            _canvas = new Texture2D(_batch.GraphicsDevice, 1, 1);
            _canvas.SetData(new[] { Color.White });
        }

        internal GraphicsDevice GraphicsDevice
        {
            get { return _batch.GraphicsDevice; }
        }

        #region Lines and Rectangles

        public void DrawLine(Point startPoint, Point endPoint, Color color, int thinkness = 1)
        {
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, color, thinkness);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color, int thinkness = 1)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;
            var length = (float)Math.Sqrt(dX * dX + dY * dY);

            _batch.Draw(_canvas, new Vector2(x1, y1), null, color, (float)Math.Atan2(dY, dX), Vector2.Zero, new Vector2(length, thinkness), SpriteEffects.None, 0);
        }

        public void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            DrawRectangle(new Rectangle(x, y, width, height), color, 0f, Vector2.Zero, true);
        }

        public void DrawRectangle(Rectangle destinationRectangle, Color color)
        {
            DrawRectangle(destinationRectangle, color, 0f, Vector2.Zero, true);
        }

        public void DrawRectangle(Rectangle destinationRectangle, Color color, float rotation = 0f, Vector2? origin = null, bool filled = true)
        {
            Vector2 useOrigin = Vector2.Zero;
            if (origin.HasValue)
                useOrigin = origin.Value;

            if (filled)
            {
                _batch.Draw(_canvas, destinationRectangle, null, color, rotation, useOrigin, SpriteEffects.None, 0f);
            }
            else
            {
                _batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, 1), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                _batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y + destinationRectangle.Height - 1, destinationRectangle.Width, 1), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                _batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y, 1, destinationRectangle.Height), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                _batch.Draw(_canvas, new Rectangle(destinationRectangle.X + destinationRectangle.Width - 1, destinationRectangle.Y, 1, destinationRectangle.Height), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
            }
        }

        #endregion

        public void DrawShape(Shape shape, Color color)
        {
            DrawShape(shape, null, color, 0f, Vector2.Zero, SpriteEffects.None);
        }

        public void DrawShape(Shape shape, Vector2 position, Color color)
        {
            var bounds = shape.Bounds;
            DrawShape(shape, new Rectangle((int)position.X, (int)position.Y, bounds.Width, bounds.Height), color, 0f, Vector2.Zero, SpriteEffects.None);
        }

        public void DrawShape(Shape shape, Rectangle destinationRectangle, Color color)
        {
            DrawShape(shape, destinationRectangle, color, 0f, Vector2.Zero, SpriteEffects.None);
        }

        public void DrawShape(Shape shape, Rectangle? destinationRectangle, Color color, float rotation = 0f, Vector2? origin = null, SpriteEffects effects = SpriteEffects.None)
        {
            Vector2 useOrigin = Vector2.Zero;
            if (origin.HasValue)
                useOrigin = origin.Value;

            Rectangle useRectangle;
            if (destinationRectangle.HasValue)
                useRectangle = destinationRectangle.Value;
            else
                useRectangle = shape.Bounds;

            if (_singleColorTextureProvider == null)
                _singleColorTextureProvider = new SingleColorShapeTextureProvider(this);

            var texture = _singleColorTextureProvider.GetTexture(shape);

            _batch.Draw(texture, useRectangle, null, color, rotation, useOrigin, effects, 0f);
        }

        public void DrawOutline(Triangle triangle, Vector2 position, Color color, int thickness = 1)
        {
            DrawLine(triangle.A, triangle.B, color, thickness);
            DrawLine(triangle.B, triangle.C, color, thickness);
            DrawLine(triangle.C, triangle.A, color, thickness);
        }

        public void DrawOutline(Polygon polygon, Vector2 position, Color color, int thickness = 1)
        {
            var points = polygon.Points;
            var offset = position.ToPoint();
            for (int i = 0; i < points.Length; i++)
            {
                Point A = points[i] + offset;
                Point B = i == points.Length - 1 ? points[0] + offset : points[i + 1] + offset; 

                DrawLine(A, B, color, thickness);
            }
        }
    }
}
