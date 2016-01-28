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
        private Matrix _projection, _view;
        private Texture2D _texture;

        private bool _projectionSet, _viewSet, _textureSet;

        public VertexPositionNormalTexture[] Vertices { get; private set; }

        public short[] Indices { get; private set; }

        public Texture2D Texture
        {
            get { return _texture; }
            set
            {
                _texture = value;
                _quadEffect.Texture = _texture;
                _textureSet = true;
            }
        }

        public Matrix Projection
        {
            get { return _projection; }
            set
            {
                _projection = value;
                _quadEffect.Projection = _projection;
                _projectionSet = true;
            }
        }

        public Matrix View
        {
            get { return _view; }
            set
            {
                _view = value;
                _quadEffect.View = _view;
                _viewSet = true;
            }
        }
        
        public TextureProjectionQuad()
        {
            Vertices = new VertexPositionNormalTexture[4];
            Indices = new short[6];

            _origin = Vector3.Zero;
            _normal = Vector3.Backward;
            _up = Vector3.Up;

            CalculateQuadCorners();
            FillVertices();
            SetupEffect();
        }

        private void CalculateQuadCorners()
        {
            float width = 1;
            float height = 1;

            _left = Vector3.Cross(_normal, _up);
            Vector3 uppercenter = (_up * height / 2) + _origin;
            _upperLeft = uppercenter + (_left * width / 2);
            _upperRight = uppercenter - (_left * width / 2);
            _lowerLeft = _upperLeft - (_up * height);
            _lowerRight = _upperRight - (_up * height);
        }

        private void FillVertices()
        {
            // Fill in texture coordinates to display the full texture
            Vector2 textureUpperLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureUpperRight = new Vector2(1.0f, 0.0f);
            Vector2 textureLowerLeft = new Vector2(0.0f, 1.0f);
            Vector2 textureLowerRight = new Vector2(1.0f, 1.0f);

            // Provide a normal for each vertex
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i].Normal = _normal;
            }

            // Set the position and texture coordinate for each
            // vertex
            Vertices[0].Position = _lowerLeft;
            Vertices[0].TextureCoordinate = textureLowerLeft;
            Vertices[1].Position = _upperLeft;
            Vertices[1].TextureCoordinate = textureUpperLeft;
            Vertices[2].Position = _lowerRight;
            Vertices[2].TextureCoordinate = textureLowerRight;
            Vertices[3].Position = _upperRight;
            Vertices[3].TextureCoordinate = textureUpperRight;

            // Set the index buffer for each vertex, using
            // clockwise winding
            Indices[0] = 0;
            Indices[1] = 1;
            Indices[2] = 2;
            Indices[3] = 2;
            Indices[4] = 1;
            Indices[5] = 3;
        }

        private void SetupEffect()
        {
            _quadEffect = new BasicEffect(Game.GraphicsDevice);
            _quadEffect.World = Matrix.Identity;
            _quadEffect.TextureEnabled = true;
            _quadEffect.EnableDefaultLighting();
        }

        public void Draw()
        {
            if (_projectionSet && _viewSet && _textureSet)
            {
                foreach (EffectPass pass in _quadEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    Game.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.TriangleList,
                        Vertices, 0, 4,
                        Indices, 0, 2);
                }
            }
            else
            {
                throw new InvalidOperationException($"The {nameof(Texture)}, {nameof(View)} and {nameof(Projection)} members of the {nameof(TextureProjectionQuad)} have to be set.");
            }
        }
    }
}
