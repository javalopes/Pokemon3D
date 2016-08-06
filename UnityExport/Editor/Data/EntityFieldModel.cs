using System;
using System.Collections.Generic;

namespace Assets.Editor.Data
{
    [Serializable]
    public class EntityFieldModel
    {
        public List<EntityFieldPositionModel> Placing = new List<EntityFieldPositionModel>();

        public EntityModel Entity;
    }
}