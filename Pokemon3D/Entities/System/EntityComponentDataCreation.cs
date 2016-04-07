using Pokemon3D.DataModel.GameMode.Map.Entities;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// A helper struct to easily move construction parameters around.
    /// </summary>
    struct EntityComponentDataCreationStruct
    {
        public string Name;
        public EntityComponentDataItemModel[] Data;
        public Entity Parent;
    }
}
