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
            if (Parent.HasComponent(IDs.CollisionSize))
                return Parent.GetComponent(IDs.CollisionSize).GetData<float>();
            else
                return 1f;
        }

        public Vector3 GetCollisionSizeVector3()
        {
            if (Parent.HasComponent(IDs.CollisionSize))
                return Parent.GetComponent(IDs.CollisionSize).GetData<Vector3>();
            else
                return Vector3.One;
        }

        public Vector3 GetCollisionOffset()
        {
            if (Parent.HasComponent(IDs.CollisionOffset))
                return Parent.GetComponent(IDs.CollisionOffset).GetData<Vector3>();
            else
                return Vector3.One;
        }
    }
}
