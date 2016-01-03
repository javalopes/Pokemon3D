using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Collisions
{
    class CollisionManager
    {
        private readonly List<Collider> _allColliders;

        public CollisionManager()
        {
            _allColliders = new List<Collider>();
        }

        public void AddCollider(Collider collider)
        {
            _allColliders.Add(collider);
        }

        public void RemoveCollider(Collider collider)
        {
            _allColliders.Remove(collider);
        }
    }
}
