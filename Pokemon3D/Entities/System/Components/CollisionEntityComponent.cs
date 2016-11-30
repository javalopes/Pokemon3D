using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Collisions;
using static GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("collision")]
    internal class CollisionEntityComponent : EntityComponent
    {
        public Collider Collider { get; private set; }
        public bool ResolvesPosition { get; set; }

        public CollisionEntityComponent(Entity referringEntity) : base(referringEntity)
        {
        }

        public CollisionEntityComponent(EntityComponentDataCreationStruct structData) : base(structData)
        {
            Collider = new Collider(GetData<Vector3>("CollisionSize"), GetData<Vector3>("CollisionOffset"));
        }

        public CollisionEntityComponent(Entity referringEntity, Vector3 collisionSize, Vector3 collisionOffset, string tag, bool resolvesPosition = false) : base(referringEntity)
        {
            ResolvesPosition = resolvesPosition;
            Collider = new Collider(collisionSize, collisionOffset) {Tag = tag};
        }

        public override void OnComponentAdded()
        {
            base.OnComponentAdded();

            if (ReferringEntity.IsInitializing || ReferringEntity.IsTemplate) return;
            Collider.SetPosition(ReferringEntity.GlobalPosition);
            GameInstance.GetService<CollisionManager>().Add(Collider);
        }

        public override void OnInitialized()
        {
            Collider.SetPosition(ReferringEntity.GlobalPosition);
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
            Collider.SetPosition(ReferringEntity.Position);

            if (!ResolvesPosition) return;
            var collisionResult = GameInstance.GetService<CollisionManager>().CheckCollision(Collider);
            if (collisionResult != null)
            {
                for (var i = 0; i < collisionResult.Length; i++)
                {
                    if (collisionResult[i].Collides) ReferringEntity.Position = ReferringEntity.Position + collisionResult[i].Axis;
                }
            }
        }
    }
}
