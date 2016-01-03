using Microsoft.Xna.Framework;

namespace Pokemon3D.Collisions
{
    /// <summary>
    /// Result of a Collision test.
    /// </summary>
    struct CollisionResult
    {
        /// <summary>
        /// Separated Axis theorem.
        /// </summary>
        public Vector3 Axis;

        /// <summary>
        /// If a collision occurs.
        /// </summary>
        public bool Collides;

        /// <summary>
        /// Default result with no collision.
        /// </summary>
        public static CollisionResult Empty = new CollisionResult
        {
            Collides = false,
            Axis = Vector3.Zero
        };
    }
}