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
        private bool _isAveraged = true;
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

        private Color _primaryColor = Color.Yellow;
        private Color _secondaryColor = Color.Blue;
        private PieChartType _type = PieChartType.FullGradient;

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
                if (_angle > MathHelper.TwoPi)
                    _angle -= MathHelper.TwoPi;

                RebuildVertices();

                if (_isAveraged)
                    _worldDirty = true;
            }
        }

        public bool IsAveraged
        {
            get { return _isAveraged; }
            set
            {
                _isAveraged = value;
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

        public Color PrimaryColor
        {
            get { return _primaryColor; }
            set
            {
                _primaryColor = value;
                RebuildVertices();
            }
        }

        public Color SecondaryColor
        {
            get { return _secondaryColor; }
            set
            {
                _secondaryColor = value;
                RebuildVertices();
            }
        }

        public PieChartType ChartType
        {
            get { return _type; }
            set
            {
                _type = value;
                RebuildVertices();
            }
        }

        public Pie2D(GraphicsDevice device, float radius, float angle, int tesselation)
            : this(device, radius, angle, tesselation, Vector2.Zero, false)
        { }

        public Pie2D(GraphicsDevice device, float radius, float angle, int tesselation, Vector2 position, bool isAveraged)
        {
            _device = device;
            _radius = radius;
            _tesselation = tesselation;
            _position = position;
            _isAveraged = isAveraged;
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
                float part;
                Color c;

                switch (_type)
                {
                    case PieChartType.SingleColor:
                        _vertices[i * 2] = new VertexPositionColor(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), _primaryColor);
                        _vertices[(i * 2) + 1] = new VertexPositionColor(Vector3.Zero, _primaryColor);
                        break;
                    case PieChartType.RadialFill:
                        _vertices[i * 2] = new VertexPositionColor(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), _primaryColor);
                        _vertices[(i * 2) + 1] = new VertexPositionColor(Vector3.Zero, _secondaryColor);
                        break;
                    case PieChartType.Gradient:
                        part = i / (float)_tesselation;
                        
                        c = new Color((byte)(_primaryColor.R + (_secondaryColor.R - _primaryColor.R) * part),
                                            (byte)(_primaryColor.G + (_secondaryColor.G - _primaryColor.G) * part),
                                            (byte)(_primaryColor.B + (_secondaryColor.B - _primaryColor.B) * part));

                        _vertices[i * 2] = new VertexPositionColor(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), c);
                        _vertices[(i * 2) + 1] = new VertexPositionColor(Vector3.Zero, c);
                        break;
                    case PieChartType.FullGradient:
                        part = i / (float)_tesselation * (_angle / MathHelper.TwoPi);

                        c = new Color((byte)(_primaryColor.R + (_secondaryColor.R - _primaryColor.R) * part),
                                            (byte)(_primaryColor.G + (_secondaryColor.G - _primaryColor.G) * part),
                                            (byte)(_primaryColor.B + (_secondaryColor.B - _primaryColor.B) * part));

                        _vertices[i * 2] = new VertexPositionColor(new Vector3((float)Math.Cos(angle), (float)Math.Sin(angle), 0), c);
                        _vertices[(i * 2) + 1] = new VertexPositionColor(Vector3.Zero, c);
                        break;
                }

            }

            if (_type == PieChartType.SingleColor)
                _vertices[_vertices.Length - 1] = new VertexPositionColor(new Vector3((float)Math.Cos(_angle), (float)Math.Sin(_angle), 0), _primaryColor);
            else
                _vertices[_vertices.Length - 1] = new VertexPositionColor(new Vector3((float)Math.Cos(_angle), (float)Math.Sin(_angle), 0), _secondaryColor);
        }

        private static float Lerp(float x0, float x1, float y0, float y1, float x2)
        {
            return y0 * (x2 - x1) / (x0 - x1) + y1 * (x2 - x0) / (x1 - x0);
        }

        public void Draw()
        {
            if (_worldDirty)
            {
                if (_isAveraged)
                    _world = Matrix.CreateScale(_radius) * Matrix.CreateRotationZ(_rotation - _angle / 2f) * Matrix.CreateTranslation(new Vector3(_position + new Vector2(_radius), 0));
                else
                    _world = Matrix.CreateScale(_radius) * Matrix.CreateRotationZ(_rotation) * Matrix.CreateTranslation(new Vector3(_position + new Vector2(_radius), 0));

                _effect.World = _world;
                _worldDirty = false;
            }

            var previousRsState = _device.RasterizerState;
            _device.RasterizerState = _rsState;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _device.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertices, 0, _tesselation * 2);
            }

            _device.RasterizerState = previousRsState;
        }

        /// <summary>
        /// Sets the angle property of the <see cref="Pie2D"/> to a multiple of Pie and the input value.
        /// </summary>
        /// <param name="part">A number between 0 and 1.</param>
        public void SetPart(float part)
        {
            _angle = (float)(MathHelper.Clamp(part, 0f, 1f) * Math.PI * 2.0);
        }
    }
}
