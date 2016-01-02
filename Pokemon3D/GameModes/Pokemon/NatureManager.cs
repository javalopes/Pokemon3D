using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.FileSystem.Requests;
using Pokemon3D.GameCore;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Natures loaded from the Natures data file.
    /// </summary>
    class NatureManager : SingleFileDataRequestModelManager<NatureModel>
    {
        public NatureManager(GameMode gameMode) : base(gameMode, gameMode.NaturesFilePath)
        { }

        public NatureModel GetNature(string id)
        {
            return _modelBuffer.Single(x => x.Id == id);
        }

        public NatureModel GetRandomNature()
        {
            return _modelBuffer[GameController.Instance.Random.Next(0, _modelBuffer.Length)];
        }
    }
}
