using System;
using System.Collections.Generic;

namespace Assets.Editor.Data
{
    [Serializable]
    public class EntityComponentModel
    {
        public string Id;

        public List<EntityComponentDataItemModel> Data = new List<EntityComponentDataItemModel>();
    }
}