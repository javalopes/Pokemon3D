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
            return BoundingBox.CollidesWithSat(other.BoundingBox);
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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
