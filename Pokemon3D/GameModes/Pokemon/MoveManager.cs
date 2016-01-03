using Pokemon3D.DataModel.Json.GameMode.Battle;
using Pokemon3D.FileSystem.Requests;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Pokémon move models.
    /// </summary>
    class MoveManager : InstantDataRequestModelManager<MoveModel>
    {
        public MoveManager(GameMode gameMode)
            : base(gameMode, gameMode.MoveFilesPath, singleModelPerFile: true)
        { }

        /// <summary>
        /// Returns a <see cref="MoveModel"/> by id.
        /// </summary>
        public MoveModel GetMoveModel(string id)
        {
            if (id == null)
                return null;

            return _modelBuffer.SingleOrDefault(x => x.Id == id);
        }
    }
}
