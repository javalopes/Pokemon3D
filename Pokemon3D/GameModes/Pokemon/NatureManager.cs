using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.GameCore;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Natures loaded from the Natures data file.
    /// </summary>
    class NatureManager : InstantDataRequestModelManager<NatureModel>
    {
        public NatureManager(GameMode gameMode) : base(gameMode, gameMode.NaturesFilePath)
        { }

        /// <summary>
        /// Returns the <see cref="NatureModel"/> for a nature id.
        /// </summary>
        public NatureModel GetNatureModel(string id)
        {
            if (id == null)
                return null;

            return _modelBuffer.SingleOrDefault(x => x.Id == id);
        }

        public NatureModel GetRandomNature()
        {
            return _modelBuffer[GameController.Instance.Random.Next(0, _modelBuffer.Length)];
        }
    }
}
