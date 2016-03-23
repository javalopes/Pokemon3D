using Microsoft.Xna.Framework;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class CollisionEntityComponent : EntityComponent
    {
        public CollisionEntityComponent(Entity parent, EntityComponentDataCreationStruct parameters) : base(parent, parameters)
        { }

        public float GetCollisionSizeSingle()
        {
            return Parent.HasComponent(IDs.CollisionSize) ? Parent.GetComponent(IDs.CollisionSize).GetData<float>() : 1f;
        }

        public Vector3 GetCollisionSizeVector3()
        {
            return Parent.HasComponent(IDs.CollisionSize) ? Parent.GetComponent(IDs.CollisionSize).GetData<Vector3>() : Parent.Scale;
        }

        public Vector3 GetCollisionOffset()
        {
            return Parent.HasComponent(IDs.CollisionOffset) ? Parent.GetComponent(IDs.CollisionOffset).GetData<Vector3>() : Vector3.Zero;
        }
    }
}
