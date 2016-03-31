using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class CollisionEntityComponent : EntityComponent
    {
        public Collider Collider { get; }
        public bool ResolvesPosition { get; set; }

        public CollisionEntityComponent(Entity parent, Vector3 collisionSize, Vector3 collisionOffset) : base(parent)
        {
            Collider = Collider.CreateBoundingBox(collisionSize, collisionOffset);
        }

        public override void OnComponentAdded()
        {
            base.OnComponentAdded();
            Parent.Game.CollisionManager.AddCollider(Collider);
        }

        public override void OnComponentRemove()
        {
            base.OnComponentRemove();
            Parent.Game.CollisionManager.RemoveCollider(Collider);
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
            Collider.SetPosition(Parent.Position);

            if (ResolvesPosition)
            {
                var collisionResult = Parent.Game.CollisionManager.CheckCollision(Collider);
                if (collisionResult != null)
                {
                    for (var i = 0; i < collisionResult.Length; i++)
                    {
                        if (collisionResult[i].Collides) Parent.Position = Parent.Position + collisionResult[i].Axis;
                    }
                }
            }
            
        }
    }
}
