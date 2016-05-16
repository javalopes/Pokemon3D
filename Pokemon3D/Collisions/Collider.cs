using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly List<Collider> _lastFrameTouched = new List<Collider>(); 
        private readonly List<Collider> _currentFrameTouched = new List<Collider>(); 

        /// <summary>
        /// Will be called when the collider enters a trigger zone.
        /// </summary>
        public Action<Collider> OnTriggerEnter;

        /// <summary>
        /// Will be called when the collider leaves the trigger zone.
        /// </summary>
        public Action<Collider> OnTriggerLeave;

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
        /// This collider can be hit and does not through away but notify.
        /// </summary>
        public bool IsTrigger { get; private set; }

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
        /// <param name="isTrigger"></param>
        public Collider(Vector3 size, Vector3? centerOffset = null, bool isTrigger = false)
        {
            IsTrigger = isTrigger;
            OffsetToCenter = centerOffset.GetValueOrDefault(Vector3.Zero);
            Type = ColliderType.BoundingBox;
            BoundingBox = new BoundingBox(-size*0.5f, size*0.5f);
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

        public void HandleTrigger(Collider other)
        {
            _currentFrameTouched.Add(other);
            if (!_lastFrameTouched.Contains(other))
            {
                OnTriggerEnter?.Invoke(other);
                _lastFrameTouched.Add(other);
            }
        }

        public void HandleUntouched()
        {
            if (_lastFrameTouched.Count == 0) return;

            var untoched = _lastFrameTouched.Except(_currentFrameTouched).ToArray();
            foreach (var untouchedCollider in untoched)
            {
                OnTriggerLeave?.Invoke(untouchedCollider);
                _lastFrameTouched.Remove(untouchedCollider);
            }

            _currentFrameTouched.Clear();
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

        public bool Intersects(Collider other)
        {
            return BoundingBox.Intersects(other.BoundingBox);
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
