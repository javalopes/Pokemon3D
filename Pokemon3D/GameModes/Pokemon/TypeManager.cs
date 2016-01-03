using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages the elemental types for Pokémon.
    /// </summary>
    class TypeManager : InstantDataRequestModelManager<TypeModel>
    {
        public TypeManager(GameMode gameMode) : base(gameMode, gameMode.TypesFilePath)
        { }

        /// <summary>
        /// Returns the <see cref="TypeModel"/> for a type id.
        /// </summary>
        public TypeModel GetTypeModel(string id)
        {
            if (id == null)
                return null;

            return _modelBuffer.SingleOrDefault(x => x.Id == id);
        }
    }
}
