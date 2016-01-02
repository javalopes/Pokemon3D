using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages the elemental types for Pokémon.
    /// </summary>
    class TypeManager : SingleFileDataRequestModelManager<TypeModel>
    {
        public TypeManager(GameMode gameMode) : base(gameMode, gameMode.TypesFilePath)
        { }

        public TypeModel GetType(string id)
        {
            return _modelBuffer.Single(x => x.Id == id);
        }
    }
}
