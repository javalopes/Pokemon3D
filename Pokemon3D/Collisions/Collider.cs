using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;

namespace Pokemon3D.Collisions
{
    /// <summary>
    /// Represents a Collision Shape.
    /// </summary>
    class Collider
    {
        private Vector3 _position;
        private Vector3 _offsetToCenter;
        private BoundingBox _boundingBox;
        private BoundingSphere _boundingSphere;

        /// <summary>
        /// Offset from Center for positioning collider to model.
        /// </summary>
        public Vector3 OffsetToCenter
        {
            get { return _offsetToCenter; }
            set
            {
                _offsetToCenter = value;
                UpdateBoundings();
            }
        }

        /// <summary>
        /// Which collision shape it is.
        /// </summary>
        public ColliderType Type { get; private set; }

        /// <summary>
        /// Corresponding BoundingBox when <see cref="Type"/> is set to box
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            private set { _boundingBox = value; }
        }

        /// <summary>
        /// Corresponding Bounding sphere when <see cref="Type"/> is set to sphere
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return _boundingSphere; }
            private set { _boundingSphere = value; }
        }

        /// <summary>
        /// Creates a bounding box collider.
        /// </summary>
        /// <param name="size">size of Bounding Box</param>
        /// <param name="centerOffset">Offset from center of object</param>
        /// <returns>Collider</returns>
        public static Collider CreateBoundingBox(Vector3 size, Vector3? centerOffset = null)
        {
            return new Collider
            {
                OffsetToCenter = centerOffset.GetValueOrDefault(Vector3.Zero),
                Type = ColliderType.BoundingBox,
                BoundingBox = new BoundingBox(-size*0.5f, size*0.5f)
            };
        }

        /// <summary>
        /// Creates a bounding sphere
        /// </summary>
        /// <param name="radius">Radius of sphere</param>
        /// <param name="centerOffset">Offset from center of object</param>
        /// <returns>Collider</returns>
        public static Collider CreateBoundingSphere(float radius, Vector3? centerOffset = null)
        {
            return new Collider
            {
                OffsetToCenter = centerOffset.GetValueOrDefault(Vector3.Zero),
                Type = ColliderType.BoundingSphere,
                BoundingSphere = new BoundingSphere(Vector3.Zero, radius)
            };
        }

        /// <summary>
        /// Sets position of Collider.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            _position = position;
            UpdateBoundings();
        }

        /// <summary>
        /// Moves Collider.
        /// </summary>
        /// <param name="offset"></param>
        public void Move(Vector3 offset)
        {
            _position += offset;
            UpdateBoundings();
        }

        /// <summary>
        /// Checks collision to another object.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public CollisionResult CheckCollision(Collider other)
        {
            switch (Type)
            {
                case ColliderType.BoundingBox:
                    if (other.Type == ColliderType.BoundingBox)
                    {
                        BoundingBox.CollidesWithSat(other.BoundingBox);
                    }
                    return new CollisionResult
                    {
                        Collides = BoundingBox.Intersects(other.BoundingSphere)
                    };
                case ColliderType.BoundingSphere:
                    if (other.Type == ColliderType.BoundingBox)
                    {
                        return new CollisionResult
                        {
                            Collides = BoundingSphere.Intersects(other.BoundingBox)
                        };
                    }
                    return new CollisionResult
                    {
                        Collides = BoundingSphere.Intersects(other.BoundingSphere)
                    };
            }

            return CollisionResult.Empty;
        }

        private void UpdateBoundings()
        {
            switch (Type)
            {
                case ColliderType.BoundingBox:
                    var size = BoundingBox.Max - BoundingBox.Min;
                    _boundingBox.Min = -size*0.5f + OffsetToCenter + _position;
                    _boundingBox.Max = size * 0.5f + OffsetToCenter + _position;
                    break;
                case ColliderType.BoundingSphere:
                    _boundingSphere.Center = _position + OffsetToCenter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
