using Pokemon3D.Common.DataHandling;
using Pokemon3D.DataModel.GameMode.Battle;
using Pokemon3D.FileSystem.Requests;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Pokémon move models.
    /// </summary>
    class MoveManager : AsyncLoadingComponent
    {
        private GameMode _gameMode;

        public MoveManager(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        public override void InitialLoadData()
        {

        }

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
