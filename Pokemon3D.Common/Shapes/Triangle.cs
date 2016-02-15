using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Shapes
{
    /// <summary>
    /// Describes a triangle.
    /// </summary>
    public struct Triangle : Shape, IEquatable<Triangle>
    {
        private Rectangle _bounds;
        private Point[] _points;

        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        public Point Location
        {
            get { return _bounds.Location; }
        }

        public Point[] Points
        {
            get { return _points; }
        }

        public Point A
        {
            get { return _points[0]; }
            set
            {
                _points[0] = value;
                CalculateBounds();
            }
        }

        public Point B
        {
            get { return _points[1]; }
            set
            {
                _points[1] = value;
                CalculateBounds();
            }
        }

        public Point C
        {
            get { return _points[2]; }
            set
            {
                _points[2] = value;
                CalculateBounds();
            }
        }

        public Triangle(Point a, Point b, Point c)
            : this(new Point[] { a, b, c })
        { }

        public Triangle(Point[] points)
        {
            if (points.Length != 3)
                throw new ArgumentException("A triangle has to have 3 points.", nameof(points));

            _points = points;
            _bounds = Rectangle.Empty;
            CalculateBounds();
        }

        private void CalculateBounds()
        {
            // recalculates the bounds of the triangle, caching it.
            // to do this, this finds the top, left, bottom and right points.

            float minX = _points.Min(p => p.X);
            float minY = _points.Min(p => p.Y);
            float maxX = _points.Max(p => p.X);
            float maxY = _points.Max(p => p.Y);
            
            _bounds = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }

        private bool IsPointInTriangle(int x, int y)
        {
            // quick out of bounds check:
            if (_bounds.X > x ||
                _bounds.Y > y ||
                _bounds.X + _bounds.Width < x ||
                _bounds.Y + _bounds.Height < y)
                return false;

            var a = _points[0];
            var b = _points[1];
            var c = _points[2];

            // We calculate the barycentric coordinates of our triangle with the point:
            var s = a.Y * c.X - a.X * c.Y + (c.Y - a.Y) * x + (a.X - c.X) * y;
            var t = a.X * b.Y - a.Y * b.X + (a.Y - b.Y) * x + (b.X - a.X) * y;

            // Then we check if those cooridinates satisfy this equation:
            // (x, y) = a + (b - a) * s + (c - a) * t

            if ((s < 0) != (t < 0))
                return false;

            var A = -b.Y * c.X + a.Y * (c.X - b.X) + a.X * (b.Y - c.Y) + b.X * c.Y;
            if (A < 0.0)
            {
                s = -s;
                t = -t;
                A = -A;
            }
            return s > 0 && t > 0 && (s + t) < A;
        }

        public bool Contains(Point value)
        {
            return IsPointInTriangle(value.X, value.Y);
        }

        public bool Contains(Rectangle value)
        {
            if (!_bounds.Contains(value))
                return false;

            if (!IsPointInTriangle(value.Left, value.Top))
                return false;

            if (!IsPointInTriangle(value.Left, value.Bottom))
                return false;

            if (!IsPointInTriangle(value.Right, value.Top))
                return false;

            if (!IsPointInTriangle(value.Right, value.Bottom))
                return false;

            return true;
        }

        public bool Contains(Vector2 value)
        {
            return IsPointInTriangle((int)value.X, (int)value.Y);
        }

        public bool Contains(int x, int y)
        {
            return IsPointInTriangle(x, y);
        }

        public double GetArea()
        {
            double area = 0;

            for (int i = 0; i < _points.Length; i++)
            {
                int i2 = i + 1;
                if (i2 > _points.Length - 1)
                    i2 = 0;

                double x0 = _points[i].X;
                double y0 = _points[i].Y;
                double x1 = _points[i2].X;
                double y1 = _points[i2].Y;

                area += (x0 - x1) * (y0 + y1) / 2;
            }

            return area;
        }

        public bool Equals(Triangle other)
        {
            return A == other.A &&
                   B == other.B &&
                   C == other.C;
        }

        public override bool Equals(object obj)
        {
            return obj is Triangle ? Equals((Triangle)obj) : false;
        }

        public override int GetHashCode()
        {
            int hash = 27;
            for (int i = 0; i < _points.Length; i++)
                hash = (13 * hash) + _points[i].GetHashCode();
            return hash;
        }

        private const string FORMAT_TRIANGLE = "{{A: {0}, B: {1}, C: {2}}}";

        public override string ToString()
        {
            return string.Format(FORMAT_TRIANGLE, _points[0].ToString(), _points[1].ToString(), _points[2].ToString());
        }

        public static bool operator ==(Triangle left, Triangle right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Triangle left, Triangle right)
        {
            return !(left.Equals(right));
        }
    }
}
