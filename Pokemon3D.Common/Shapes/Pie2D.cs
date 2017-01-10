using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Renders a 2D pie chart.
    /// </summary>
    public class Pie2D
    {
        private float _rotation;
        private float _angle;
        private float _radius;
        private int _tesselation;
        private bool _isAveraged;
        private bool _worldDirty = true;
        private bool _verticesDirty = true;

        private Vector2 _position;
        private VertexPositionColor[] _vertices;
        private Matrix _world;
        private readonly RasterizerState _rsState =
            new RasterizerState()
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

        private Color _primaryColor = Color.White;
        private Color _secondaryColor = Color.White;
        private PieChartType _type = PieChartType.SingleColor;

        private BasicEffect _effect;
        private readonly GraphicsDevice _device;

        /// <summary>
        /// The rotation in radians of the chart.
        /// </summary>
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

        /// <summary>
        /// The part of the chart that is visible. Completely invisible at 0.
        /// </summary>
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                if (_angle > MathHelper.TwoPi)
                    _angle -= MathHelper.TwoPi;

                _verticesDirty = true;

                if (_isAveraged)
                    _worldDirty = true;
            }
        }

        /// <summary>
        /// If the chart has a set starting point of if it changes depending on the angle.
        /// </summary>
        public bool IsAveraged
        {
            get { return _isAveraged; }
            set
            {
                _isAveraged = value;
                _worldDirty = true;
            }
        }

        /// <summary>
        /// The position of the chart on the screen.
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _worldDirty = true;
            }
        }

        /// <summary>
        /// The amount of vertices used to represent the chart.
        /// </summary>
        public int Tesselation
        {
            get { return _tesselation; }
            set
            {
                _tesselation = value;
                _verticesDirty = true;
            }
        }

        public Color PrimaryColor
        {
            get { return _primaryColor; }
            set
            {
                _primaryColor = value;
                _verticesDirty = true;
            }
        }

        public Color SecondaryColor
        {
            get { return _secondaryColor; }
            set
            {
                _secondaryColor = value;
                _verticesDirty = true;
            }
        }

        public PieChartType ChartType
        {
            get { return _type; }
            set
            {
                _type = value;
                _verticesDirty = true;
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

            _effect.View = Matrix.CreateLookAt(Vector3.Backward, Vector3.Zero, Vector3.Up);
            _effect.VertexColorEnabled = true;
            _effect.LightingEnabled = false;
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

        private RenderTarget2D _target;
        private bool _isRenderTargetMode;

        public void Draw()
        {
            _isRenderTargetMode = false;
            DrawInternal();
        }

        public void DrawBatched(SpriteBatch batch)
        {
            int diameter = (int)(_radius * 2f);
            if (_target == null || _target.Width != diameter)
                _target = new RenderTarget2D(_device, diameter, diameter);

            var tempPosition = _position;
            _position = Vector2.Zero;

            var prevTargets = _device.GetRenderTargets();

            _device.SetRenderTarget(_target);
            _device.Clear(Color.Transparent);

            _isRenderTargetMode = true;
            DrawInternal();

            _device.SetRenderTargets(prevTargets);

            _position = tempPosition;

            batch.Draw(_target, _position, Color.White);
        }

        private void DrawInternal()
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
            if (_verticesDirty)
            {
                RebuildVertices();
                _verticesDirty = false;
            }

            if (_isRenderTargetMode)
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0, _target.Width, _target.Height, 0, 1, 50);
            else
            {
                var pp = _device.PresentationParameters;
                _effect.Projection = Matrix.CreateOrthographicOffCenter(0, pp.BackBufferWidth, pp.BackBufferHeight, 0, 1, 50);
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
            Angle = MathHelper.Clamp(part, 0f, 1f) * MathHelper.TwoPi;
        }
    }
}
