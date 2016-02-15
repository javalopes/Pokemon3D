using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Creates <see cref="Texture2D"/> for a <see cref="Shape"/>.
    /// </summary>
    abstract class ShapeTextureProvider
    {
        protected Dictionary<int, Texture2D> _buffer = new Dictionary<int, Texture2D>();
        protected ShapeRenderer _renderer;

        public ShapeTextureProvider(ShapeRenderer renderer)
        {
            _renderer = renderer;
        }

        public Texture2D GetTexture(Shape shape)
        {
            int hash = shape.GetHashCode();

            Texture2D texture;

            if (!_buffer.TryGetValue(hash, out texture))
            {
                texture = CreateTexture(shape);
                _buffer.Add(hash, texture);
            }

            return texture;
        }

        protected abstract Texture2D CreateTexture(Shape shape);
    }
}
