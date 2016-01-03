using Microsoft.Xna.Framework;

namespace Pokemon3D.Collisions
{
    static class BoundingBoxExtensions
    {
        private static Line1D GetProjectedPoints(BoundingBox boundingBox, Vector3 axis)
        {
            var points = boundingBox.GetCorners();

            var min = Vector3.Dot(axis, points[0]);
            var max = min;
            for (var i = 1; i < points.Length; i++)
            {
                var p = Vector3.Dot(axis, points[i]);
                if (p < min)
                {
                    min = p;
                }
                else if (p > max)
                {
                    max = p;
                }
            }

            return new Line1D(min, max);
        }

        public static CollisionResult CollidesWithSat(this BoundingBox boundingBox, BoundingBox other)
        {
            var axis = new[]
            {
                Vector3.UnitX,
                Vector3.UnitY,
                Vector3.UnitZ
            };

            var shortestAxis = Vector3.Zero;
            var shortestOverlap = float.MaxValue;

            for (var i = 0; i < axis.Length; i++)
            {
                var currentAxis = axis[i];
                var p1 = GetProjectedPoints(boundingBox, currentAxis);
                var p2 = GetProjectedPoints(other, currentAxis);

                var overlapValue = p1.OverlapSize(p2);

                if (overlapValue > 0.0f)
                {
                    if (overlapValue < shortestOverlap)
                    {
                        shortestOverlap = overlapValue;
                        shortestAxis = currentAxis;
                    }
                }
                else
                {
                    return CollisionResult.Empty;
                }
            }

            return new CollisionResult
            {
                Collides = true,
                Axis = shortestAxis * shortestOverlap
            };
        }
    }
}
