using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("collision")]
    internal class CollisionEntityComponent : EntityComponent
    {
        public Collider Collider { get; private set; }
        public bool ResolvesPosition { get; set; }

        public CollisionEntityComponent(Entity parent) : base(parent)
        {
        }

        public CollisionEntityComponent(EntityComponentDataCreationStruct structData) : base(structData)
        {
            Collider = new Collider(GetData<Vector3>("CollisionSize"), GetData<Vector3>("CollisionOffset"));
        }

        public CollisionEntityComponent(Entity parent, Vector3 collisionSize, Vector3 collisionOffset, string tag) : base(parent)
        {
            Collider = new Collider(collisionSize, collisionOffset) {Tag = tag};
        }

        public override void OnComponentAdded()
        {
            base.OnComponentAdded();

            if (Parent.IsInitializing || Parent.IsTemplate) return;
            Collider.SetPosition(Parent.GlobalPosition);
            GameInstance.GetService<CollisionManager>().Add(Collider);
        }

        public override void OnInitialized()
        {
            Collider.SetPosition(Parent.GlobalPosition);
            GameInstance.GetService<CollisionManager>().Add(Collider);
        }

        public override EntityComponent Clone(Entity target)
        {
            return new CollisionEntityComponent(target)
            {
                Collider = Collider.Clone(),
                ResolvesPosition = ResolvesPosition,
                IsActive = target.IsActive
            };
        }

        public override void OnComponentRemove()
        {
            GameInstance.GetService<CollisionManager>().Remove(Collider);
        }

        public override void OnIsActiveChanged()
        {
            Collider.IsActive = IsActive;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Collider.SetPosition(Parent.Position);

            if (!ResolvesPosition) return;
            var collisionResult = GameInstance.GetService<CollisionManager>().CheckCollision(Collider);
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
