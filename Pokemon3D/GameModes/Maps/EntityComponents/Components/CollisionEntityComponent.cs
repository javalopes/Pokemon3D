using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Maps.EntityComponents.Components
{
    class CollisionEntityComponent : EntityComponent
    {
        public CollisionEntityComponent(EntityComponentDataCreationStruct parameters) : base(parameters)
        { }

        public float GetCollisionSizeSingle()
        {
            return Parent.HasComponent(IDs.CollisionSize) ? Parent.GetComponent(IDs.CollisionSize).GetData<float>() : 1f;
        }

        public Vector3 GetCollisionSizeVector3()
        {
            if (Parent.HasComponent(IDs.CollisionSize))
                return Parent.GetComponent(IDs.CollisionSize).GetData<Vector3>();
            else
                return Parent.Scale;
        }

        public Vector3 GetCollisionOffset()
        {
            return Parent.HasComponent(IDs.CollisionOffset) ? Parent.GetComponent(IDs.CollisionOffset).GetData<Vector3>() : Vector3.One;
            if (Parent.HasComponent(IDs.CollisionOffset))
                return Parent.GetComponent(IDs.CollisionOffset).GetData<Vector3>();
            else
                return Vector3.Zero;
        }
    }
}
