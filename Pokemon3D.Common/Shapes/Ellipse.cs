using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Describes an ellipse.
    /// </summary>
    public struct Ellipse : Shape, IEquatable<Ellipse>
    {
        private Rectangle _bounds;

        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        public Ellipse(int x, int y, int width, int height)
        {
            _bounds = new Rectangle(x, y, width, height);
        }

        public Ellipse(Rectangle bounds)
        {
            _bounds = bounds;
        }

        private bool IsPointInEllipse(int x, int y)
        {
            // quick out of bounds check:
            if (_bounds.X > x ||
                _bounds.Y > y ||
                _bounds.X + _bounds.Width < x ||
                _bounds.Y + _bounds.Height < y)
                return false;

            double xRadius = _bounds.Width / 2d;
            double yRadius = _bounds.Height / 2d;

            // if the radius is smaller than or equal to 0, the point cannot possibly be inside the ellipse, as it does not have any area.
            if (xRadius <= 0f || yRadius <= 0f)
                return false;

            int normalizedX = x - _bounds.Center.X;
            int normalizedY = y - _bounds.Center.Y;

            // circle equation:
            // X^2/a^2 + Y^2/b^2 <= 1
            return ((normalizedX * normalizedX) / (xRadius * xRadius)) + ((normalizedY * normalizedY) / (yRadius * yRadius)) <= 1.0;
        }

        public bool Contains(Point value)
        {
            return IsPointInEllipse(value.X, value.Y);
        }
        
        public bool Contains(Rectangle value)
        {
            //Check if the rectangle is inside the bounds.
            if (!_bounds.Contains(value))
                return false;

            //Check if the points of the rectangle are inside the ellipse.
            if (!IsPointInEllipse(value.Left, value.Top))
                return false;

            if (!IsPointInEllipse(value.Left, value.Bottom))
                return false;

            if (!IsPointInEllipse(value.Right, value.Top))
                return false;

            if (!IsPointInEllipse(value.Right, value.Bottom))
                return false;

            //If all corner points of the rectangle are inside the ellipse,
            //the rectangle also is.

            return true;
        }

        public bool Contains(Vector2 value)
        {
            return IsPointInEllipse((int)value.X, (int)value.Y);
        }

        public bool Contains(int x, int y)
        {
            return IsPointInEllipse(x, y);
        }

        public double GetArea()
        {
            return MathHelper.Pi * 
                _bounds.Width * 
                _bounds.Height;
        }

        public bool Equals(Ellipse other)
        {
            return _bounds == other._bounds;
        }

        public override bool Equals(object obj)
        {
            return obj is Ellipse ? Equals((Ellipse)obj) : false;
        }

        public override int GetHashCode()
        {
            int hash = 27;
            hash = (13 * hash) + _bounds.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return _bounds.ToString();
        }

        public static bool operator ==(Ellipse left, Ellipse right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Ellipse left, Ellipse right)
        {
            return !(left.Equals(right));
        }
    }
}
