using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Shapes
{
    public class ShapeRenderer
    {
        private const int EllipseSegmentCount = 36;

        private readonly Texture2D _canvas;
        public SpriteBatch Batch { get; }

        private readonly BasicEffect _basicEffect;
        private readonly Mesh _unitCircleMesh;

        public ShapeRenderer(SpriteBatch spriteBatch)
        {
            Batch = spriteBatch;
            _canvas = new Texture2D(Batch.GraphicsDevice, 1, 1);
            _canvas.SetData(new[] { Color.White });

            _basicEffect = new BasicEffect(GraphicsDevice)
            {
                LightingEnabled = false,
                FogEnabled = false,
                PreferPerPixelLighting = false,
                TextureEnabled = true,
                Texture = _canvas
            };

            var vertices = new List<VertexPositionNormalTexture>
            {
                new VertexPositionNormalTexture(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, new Vector2(0.5f))
            };
            var indices = new List<ushort>();

            for(var i = 0; i < EllipseSegmentCount; i++)
            {
                var angle = MathHelper.TwoPi * (i/ (float) EllipseSegmentCount);
                vertices.Add(new VertexPositionNormalTexture(new Vector3((float) Math.Cos(angle), (float) Math.Sin(angle), 1.0f), Vector3.Zero, new Vector2(0.5f)));

                indices.Add(0);
                indices.Add((ushort) (i+2));
                indices.Add((ushort) (i+1));
            }

            var geometry = new GeometryData
            {
                Vertices = vertices.ToArray(),
                Indices = indices.ToArray(),
                PrimitiveType = PrimitiveType.TriangleList
            };

            _unitCircleMesh = new Mesh(GraphicsDevice, geometry, false);
        }

        internal GraphicsDevice GraphicsDevice => Batch.GraphicsDevice;

        public void DrawLine(Point startPoint, Point endPoint, Color color, int thickness = 1)
        {
            DrawLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, color, thickness);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Color color, int thickness = 1)
        {
            var dX = x2 - x1;
            var dY = y2 - y1;
            var length = (float)Math.Sqrt(dX * dX + dY * dY);

            Batch.Draw(_canvas, new Vector2(x1, y1), null, color, (float)Math.Atan2(dY, dX), Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }

        public void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            DrawRectangle(new Rectangle(x, y, width, height), color, 0f, Vector2.Zero);
        }

        public void DrawRectangle(Rectangle destinationRectangle, Color color)
        {
            DrawRectangle(destinationRectangle, color, 0f, Vector2.Zero);
        }

        public void DrawRectangle(Rectangle destinationRectangle, Color color, float rotation, Vector2? origin, bool filled = true)
        {
            var useOrigin = origin.GetValueOrDefault(Vector2.Zero);

            if (filled)
            {
                Batch.Draw(_canvas, destinationRectangle, null, color, rotation, useOrigin, SpriteEffects.None, 0f);
            }
            else
            {
                Batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, 1), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                Batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y + destinationRectangle.Height - 1, destinationRectangle.Width, 1), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                Batch.Draw(_canvas, new Rectangle(destinationRectangle.X, destinationRectangle.Y, 1, destinationRectangle.Height), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
                Batch.Draw(_canvas, new Rectangle(destinationRectangle.X + destinationRectangle.Width - 1, destinationRectangle.Y, 1, destinationRectangle.Height), null, color, rotation, useOrigin, SpriteEffects.None, 0f);
            }
        }

        public void DrawEllipse(Ellipse ellipse, Color color)
        {
            DrawEllipsePie(ellipse, color, MathHelper.TwoPi);
        }

        public void DrawEllipsePie(Ellipse ellipse, Color color, float angle)
        {
            var oldStateDepth = GraphicsDevice.DepthStencilState;
            var oldStateBlend = GraphicsDevice.BlendState;

            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            var percentage = angle / MathHelper.TwoPi;
            var segmentCount = (int)Math.Round(EllipseSegmentCount * percentage, MidpointRounding.AwayFromZero);

            if (segmentCount == 0) return;

            _basicEffect.World = Matrix.CreateScale(ellipse.Bounds.Width, ellipse.Bounds.Height, 1.0f) *
                                 Matrix.CreateTranslation(new Vector3(ellipse.Location.ToVector2(), 0.0f));
            _basicEffect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up);
            _basicEffect.Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 10.0f);
            _basicEffect.DiffuseColor = color.ToVector3();

            foreach (var pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _unitCircleMesh.Draw(segmentCount);
            }

            GraphicsDevice.DepthStencilState = oldStateDepth;
            GraphicsDevice.BlendState = oldStateBlend;
        }
    }
}
