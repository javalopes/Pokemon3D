using System;
using System.Collections.Generic;

namespace Assets.Editor.Data
{
    [Serializable]
    public class EntityModel 
    {
        public string Id;

        public string Generator;

        public List<EntityComponentModel> Components = new List<EntityComponentModel>();

        public bool IsStatic;
    }
}