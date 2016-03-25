using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Renders a 2D pie chart.
    /// </summary>
    public class Pie2D
    {
        private float _rotation = 0f;
        private float _angle = 0f;
        private float _radius = 0f;
        private int _tesselation = 0;
        private bool _centered = true;
        private bool _worldDirty = true;

        private Vector2 _position;
        private VertexPositionColor[] _vertices;
        private Matrix _world;
        private RasterizerState _rsState =
            new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

        private BasicEffect _effect;
        private GraphicsDevice _device;

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _worldDirty = true;
            }
        }

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                _worldDirty = true;
            }
        }

        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                RebuildVertices();
            }
        }

        public bool IsCentered
        {
            get { return _centered; }
            set
            {
                _centered = value;
                _worldDirty = true;
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _worldDirty = true;
            }
        }

        public int Tesselation
        {
            get { return _tesselation; }
            set
            {
                _tesselation = value;
                RebuildVertices();
            }
        }

        public Pie2D(GraphicsDevice device, float radius, float angle, int tesselation)
            : this(device, radius, angle, tesselation, Vector2.Zero, false)
        { }

        public Pie2D(GraphicsDevice device, float radius, float angle, int tesselation, Vector2 position, bool centered)
        {
            _device = device;
            _radius = radius;
            _tesselation = tesselation;
            _position = position;
            _centered = centered;
            _angle = angle;

            Initialize();
        }

        private void Initialize()
        {
            _effect = new BasicEffect(_device);

            var pp = _device.PresentationParameters;
            _effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up);
            _effect.Projection = Matrix.CreateOrthographicOffCenter(0, pp.BackBufferWidth, pp.BackBufferHeight, 0, 1, 50);
            _effect.VertexColorEnabled = true;
            _effect.LightingEnabled = false;

            RebuildVertices();
        }

        private void RebuildVertices()
        {
            _vertices = new VertexPositionColor[_tesselation * 4 + 1];
            for (int i = 0; i < _tesselation * 2; i++)
            {
                float angle = Lerp(0, _tesselation, 0, _angle, i);
                _vertices[i * 2] = new VertexPositionColor(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), Color.Red);
                _vertices[(i * 2) + 1] = new VertexPositionColor(Vector3.Zero, Color.Red);
            }
            _vertices[_vertices.Length - 1] = new VertexPositionColor(new Vector3((float)Math.Cos(_angle), (float)Math.Sin(_angle), 0), Color.Red);
        }

        private static float Lerp(float x0, float x1, float y0, float y1, float x2)
        {
            return y0 * (x2 - x1) / (x0 - x1) + y1 * (x2 - x0) / (x1 - x0);
        }

        public void Draw()
        {
            if (_worldDirty)
            {
                if (_centered)
                    _world = Matrix.CreateScale(_radius) * Matrix.CreateRotationZ(_rotation - _angle / 2f) * Matrix.CreateTranslation(new Vector3(_position, 0));
                else
                    _world = Matrix.CreateScale(_radius) * Matrix.CreateRotationZ(_rotation) * Matrix.CreateTranslation(new Vector3(_position, 0));

                _worldDirty = false;
            }

            _effect.World = _world;

            var previousRsState = _device.RasterizerState;
            _device.RasterizerState = _rsState;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _device.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, _vertices, 0, _tesselation * 2);
            }

            _device.RasterizerState = previousRsState;
        }
    }
}
