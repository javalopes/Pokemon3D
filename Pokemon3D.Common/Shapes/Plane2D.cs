using Microsoft.Xna.Framework;
using System;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Descriptes a rectangular plane.
    /// </summary>
    public struct Plane2D : Shape, IEquatable<Plane2D>
    {
        private Rectangle _bounds;

        /// <summary>
        /// Gets or sets the bounds of this <see cref="Plane2D"/>.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the location of this <see cref="Plane2D"/>.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Plane2D"/> struct.
        /// </summary>
        public Plane2D(Rectangle bounds)
        {
            _bounds = bounds;
        }

        public Plane2D(int x, int y, int width, int height)
        {
            _bounds = new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Returns the area of this <see cref="Plane2D"/>.
        /// </summary>
        public double GetArea()
        {
            return _bounds.Width * _bounds.Height;
        }

        /// <summary>
        /// Checks if a this <see cref="Plane2D"/> contains a <see cref="Point"/>.
        /// </summary>
        public bool Contains(Point value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane2D"/> contains a <see cref="Rectangle"/>.
        /// </summary>
        public bool Contains(Rectangle value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane2D"/> contains a <see cref="Vector2"/>.
        /// </summary>
        public bool Contains(Vector2 value)
        {
            return _bounds.Contains(value);
        }

        /// <summary>
        /// Checks if a this <see cref="Plane2D"/> contains the specified coordinates.
        /// </summary>
        public bool Contains(int x, int y)
        {
            return _bounds.Contains(x, y);
        }

        /// <summary>
        /// Compares whether the current instance is equal to specified <see cref="Plane2D"/>.
        /// </summary>
        public bool Equals(Plane2D other)
        {
            return _bounds.Equals(other._bounds);
        }

        /// <summary>
        /// Compares if this instance is equal to an object.
        /// </summary>
        public override bool Equals(object obj)
        {
            return obj is Plane2D ? Equals((Plane2D)obj) : false;
        }

        /// <summary>
        /// Get the hash code of this <see cref="Plane2D"/>.
        /// </summary>
        public override int GetHashCode()
        {
            int hash = 27;
            hash = (hash * 13) + _bounds.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Plane2D"/>.
        /// </summary>
        public override string ToString()
        {
            return _bounds.ToString();
        }

        /// <summary>
        /// Compares whether two <see cref="Plane2D"/> instances are equal.
        /// </summary>
        public static bool operator ==(Plane2D left, Plane2D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares whether two <see cref="Plane2D"/> instances are not equal.
        /// </summary>
        public static bool operator !=(Plane2D left, Plane2D right)
        {
            return !left.Equals(right);
        }
    }
}
