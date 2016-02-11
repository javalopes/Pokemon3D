using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.UI.Framework.Shapes
{
    class Polygon
    {
        public List<Vector2> Vertices { get; set; }

        public Rectangle Bounds
        {
            get
            {
                float minX = Vertices.Min(v => v.X);
                float minY = Vertices.Min(v => v.Y);
                float maxX = Vertices.Max(v => v.X);
                float maxY = Vertices.Max(v => v.Y);

                return new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
            }
        }

        public Polygon()
        {
            Vertices = new List<Vector2>();
        }

        public bool Intersects(Point point)
        {
            return Intersects(point.ToVector2());
        }

        public bool Intersects(Vector2 vector)
        {
            // check bounds first:

            if (Bounds.Contains(vector))
            {
                bool inside = false;

                for (int i = 0, j = Vertices.Count - 1; i < Vertices.Count; j = i++)
                {
                    if (((Vertices[i].Y > vector.Y) != (Vertices[j].Y > vector.Y)) &&
                        (vector.X < (Vertices[j].X - Vertices[i].X) * (vector.Y - Vertices[i].Y) / (Vertices[j].Y - Vertices[i].Y) + Vertices[i].X))
                    {
                        inside = !inside;
                    }
                }

                return inside;
            }
            else
            {
                return false;
            }
        }
    }
}
