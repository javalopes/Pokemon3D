using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Shapes
{
    public struct Polygon : Shape, IEquatable<Polygon>
    {
        private Rectangle _bounds;
        private List<Point> _points;

        public Rectangle Bounds
        {
            get { return _bounds; }
        }

        public Point Location
        {
            get { return _bounds.Location; }
            set
            {
                Point offset = value - _bounds.Location;
                for (int i = 0; i < _points.Count; i++)
                    _points[i] += offset;

                _bounds.Location = value;
            }
        }

        public Point[] Points
        {
            get { return _points.ToArray(); }
        }

        public int Length
        {
            get { return _points.Count; }
        }

        public Polygon(Point[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("A Polygon has to consist of at least three vertices.");

            _points = points.ToList();
            _bounds = Rectangle.Empty;

            CalculateBounds();
        }

        private void CalculateBounds()
        {
            float minX = _points.Min(v => v.X);
            float minY = _points.Min(v => v.Y);
            float maxX = _points.Max(v => v.X);
            float maxY = _points.Max(v => v.Y);

            _bounds =  new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
        }

        private bool IsPointInPolygon(int x, int y)
        {
            // quick out of bounds check:
            if (_bounds.X > x ||
                _bounds.Y > y ||
                _bounds.X + _bounds.Width < x ||
                _bounds.Y + _bounds.Height < y)
                return false;

            bool isInside = false;

            for (int i = 0, j = _points.Count - 1; i < _points.Count; j = i++)
            {
                if (((_points[i].Y > y) != (_points[j].Y > y)) &&
                (x < (_points[j].X - _points[i].X) * (y - _points[i].Y) / (_points[j].Y - _points[i].Y) + _points[i].X))
                {
                    isInside = !isInside;
                }
            }

            return isInside;
        }

        public void Add(Point point)
        {
            _points.Add(point);
            CalculateBounds();
        }

        public void Insert(int index, Point point)
        {
            _points.Insert(index, point);
            CalculateBounds();
        }

        public void Remove(Point point)
        {
            if (_points.Contains(point))
                Remove(_points.IndexOf(point));
            else
                throw new InvalidOperationException("The given point does not exist as a point of this polygon.");
        }

        public void Remove(int index)
        {
            if (_points.Count <= index || index < 0)
                throw new IndexOutOfRangeException();
            else if (_points.Count == 3)
                throw new ArgumentException("A polygon has to consist of at least three vertices.");
            else
            {
                _points.RemoveAt(index);
                CalculateBounds();
            }
        }

        public bool Contains(Point value)
        {
            return IsPointInPolygon(value.X, value.Y);
        }

        public bool Contains(Rectangle value)
        {
            //Check if the rectangle is inside the bounds.
            if (!_bounds.Contains(value))
                return false;

            //Check if the points of the rectangle are inside the polygon.
            if (!IsPointInPolygon(value.Left, value.Top))
                return false;

            if (!IsPointInPolygon(value.Left, value.Bottom))
                return false;

            if (!IsPointInPolygon(value.Right, value.Top))
                return false;

            if (!IsPointInPolygon(value.Right, value.Bottom))
                return false;

            return true;
        }

        public bool Contains(Vector2 value)
        {
            return IsPointInPolygon((int)value.X, (int)value.Y);
        }

        public bool Contains(int x, int y)
        {
            return IsPointInPolygon(x, y);
        }

        public double GetArea()
        {
            double area = 0;

            for (int i = 0; i < _points.Count; i++)
            {
                int i2 = i + 1;
                if (i2 > _points.Count - 1)
                    i2 = 0;

                double x0 = _points[i].X;
                double y0 = _points[i].Y;
                double x1 = _points[i2].X;
                double y1 = _points[i2].Y;

                area += (x0 - x1) * (y0 + y1) / 2;
            }

            return area;
        }

        public bool Equals(Polygon other)
        {
            if (other.Length != Length)
                return false;

            for (int i = 0; i < _points.Count; i++)
                if (_points[i] != other._points[i])
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Polygon ? Equals((Polygon)obj) : false;
        }

        public override int GetHashCode()
        {
            int hash = 27;

            for (int i = 0; i < _points.Count; i++)
                hash = (13 * hash) + _points[i].GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("{");

            for (int i = 0; i < _points.Count; i++)
            {
                if (i > 0)
                    sb.Append(", ");

                sb.Append(i.ToString());
                sb.Append(": ");
                sb.Append(_points[i].ToString());
            }

            sb.Append("}");

            return sb.ToString();
        }

        public static bool operator ==(Polygon left, Polygon right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Polygon left, Polygon right)
        {
            return !(left.Equals(right));
        }
    }
}
