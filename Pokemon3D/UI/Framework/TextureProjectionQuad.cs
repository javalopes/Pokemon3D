using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Draws a projection of a texture on a 3D quad.
    /// </summary>
    class TextureProjectionQuad : GameObject
    {
        private Vector3 _origin, _upperLeft, _lowerLeft, _upperRight, _lowerRight, _normal, _up, _left;
        private BasicEffect _quadEffect;
        private RenderTarget2D _target;
        private Matrix _projection, _view;
        private VertexPositionNormalTexture[] _vertices;
        private short[] _indices;
        private bool _viewDirty = true;
        private bool _projectionDirty = true;
        private bool _targetDirty = true;
        private float _fieldOfView = 45;
        private int _textureOutputWidth = 0;
        private int _textureOutputHeight = 0;
        private Vector3 _cameraPosition = Vector3.Zero;

        public Matrix World { get; set; } = Matrix.Identity;

        public Texture2D Texture
        {
            get { return _quadEffect.Texture; }
            set
            {
                _quadEffect.Texture = value;
            }
        }

        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                _fieldOfView = value;
                _projectionDirty = true;
            }
        }

        public int TextureOutputWidth
        {
            get { return _textureOutputWidth; }
            set
            {
                _textureOutputWidth = value;
                _projectionDirty = true;
                _targetDirty = true;
            }
        }

        public int TextureOutputHeight
        {
            get { return _textureOutputHeight; }
            set
            {
                _textureOutputHeight = value;
                _projectionDirty = true;
                _targetDirty = true;
            }
        }

        public Vector3 CameraPosition
        {
            get { return _cameraPosition; }
            set
            {
                _cameraPosition = value;
                _viewDirty = true;
            }
        }

        public TextureProjectionQuad()
        {
            _vertices = new VertexPositionNormalTexture[4];
            _indices = new short[6];

            _origin = Vector3.Zero;
            _normal = Vector3.Backward;
            _up = Vector3.Up;

            CalculateQuadCorners();
            FillVertices();
            SetupEffect();
        }

        private void CalculateQuadCorners()
        {
            _left = Vector3.Cross(_normal, _up);
            Vector3 uppercenter = (_up / 2) + _origin;
            _upperLeft = uppercenter + (_left / 2);
            _upperRight = uppercenter - (_left / 2);
            _lowerLeft = _upperLeft - (_up);
            _lowerRight = _upperRight - (_up);
        }

        private void FillVertices()
        {
            // Fill in texture coordinates to display the full texture
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < _vertices.Length; i++)
            {
                _vertices[i].Normal = _normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            _vertices[0].Position = _lowerLeft;
            _vertices[0].TextureCoordinate = textureLowerLeft;
            _vertices[1].Position = _upperLeft;
            _vertices[1].TextureCoordinate = textureUpperLeft;
            _vertices[2].Position = _lowerRight;
            _vertices[2].TextureCoordinate = textureLowerRight;
            _vertices[3].Position = _upperRight;
            _vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            _indices[0] = 0;
            _indices[1] = 1;
            _indices[2] = 2;
            _indices[3] = 2;
            _indices[4] = 1;
            _indices[5] = 3;
        }

        private void SetupEffect()
        {
            _quadEffect = new BasicEffect(Game.GraphicsDevice);
            _quadEffect.TextureEnabled = true;
        }

        public Texture2D GetProjected()
        {
            if (_quadEffect.Texture != null)
            {
                if (_projectionDirty)
                {
                    _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(_fieldOfView), _textureOutputWidth / _textureOutputHeight, 0.01f, 10000f);
                    _quadEffect.Projection = _projection;
                    _projectionDirty = false;
                }
                if (_viewDirty)
                {
                    _view = Matrix.CreateLookAt(_cameraPosition, Vector3.Zero, Vector3.Up);
                    _quadEffect.View = _view;
                    _viewDirty = false;
                }
                if (_targetDirty)
                {
                    _target = new RenderTarget2D(Game.GraphicsDevice, _textureOutputWidth, _textureOutputHeight);
                    _targetDirty = false;
                }

                _quadEffect.World = World;

                var prevTargets = Game.GraphicsDevice.GetRenderTargets();
                Game.GraphicsDevice.SetRenderTarget(_target);
                Game.GraphicsDevice.Clear(Color.Transparent);

                foreach (EffectPass pass in _quadEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    Game.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        _vertices, 0, 4,
                        _indices, 0, 2);
                }

                Game.GraphicsDevice.SetRenderTargets(prevTargets);
                return _target;
            }
            else
            {
                throw new InvalidOperationException($"The {nameof(Texture)} member of the {nameof(TextureProjectionQuad)} has to be set.");
            }
        }

        /// <summary>
        /// Projects a <see cref="Vector2"/> from screen to world space.
        /// </summary>
        public Vector2 ProjectVector2(Vector2 source)
        {
            var projectVector = new Vector3(
                -0.5f + (source.X / _textureOutputWidth), 
                0.5f - (source.Y / _textureOutputHeight),
                0f); // disregard depth value

            var projected = Project(projectVector, _projection, _view, World);

            return new Vector2(projected.X, projected.Y); // disregard depth value
        }

        public Polygon ProjectRectangle(Rectangle rectangle)
        {
            Polygon polygon = new Polygon();
            
            // get corners counter clockwise starting on the top left and project them to create the polygon:
            Vertices.AddRange(new Vector2[] {
                new Vector2(rectangle.X, rectangle.Y), // top left
                new Vector2(rectangle.X, rectangle.Y + rectangle.Height), // bottom left
                new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), // bottom right
                new Vector2(rectangle.X + rectangle.Width, rectangle.Y) // top right
            }.Select(v => ProjectVector2(v)));
            
            return polygon;
        }

        private Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            var viewport = Game.GraphicsDevice.Viewport;
            int x = viewport.X;
            int y = viewport.Y;
            int width = TextureOutputWidth;
            int height = TextureOutputHeight;
            float maxDepth = viewport.MaxDepth;
            float minDepth = viewport.MinDepth;

            Matrix matrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            Vector3 vector = Vector3.Transform(source, matrix);
            float a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector.X = vector.X / a;
                vector.Y = vector.Y / a;
                vector.Z = vector.Z / a;
            }
            vector.X = (((vector.X + 1f) * 0.5f) * width) + x;
            vector.Y = (((-vector.Y + 1f) * 0.5f) * height) + y;
            vector.Z = (vector.Z * (maxDepth - minDepth)) + minDepth;
            return vector;
        }

        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;
            return ((-1.401298E-45f <= num) && (num <= float.Epsilon));
        }
    }
}
