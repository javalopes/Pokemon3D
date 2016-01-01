using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode.Definitions;
using Pokemon3D.FileSystem;
using Pokemon3D.GameCore;
using System;
using System.Linq;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Natures loaded from the Natures data file.
    /// </summary>
    class NatureManager
    {
        private GameMode _gameMode;
        private NatureModel[] _natures;

        public bool FinishedLoading { get; private set; } = false;

        public NatureManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            InitiateDataRequest();
        }

        private void InitiateDataRequest()
        {
            var request = new DataRequest(_gameMode, _gameMode.NaturesFilePath);
            request.Finished += RequestFinished;
            request.StartThreaded();
        }

        private void RequestFinished(object sender, EventArgs e)
        {
            var request = (DataRequest)sender;
            _natures = DataModel<NatureModel[]>.FromString(request.ResultData);
            FinishedLoading = true;
        }

        public NatureModel GetNature(string natureId)
        {
            return _natures.Single(x => x.Id == natureId);
        }

        public NatureModel GetRandomNature()
        {
            return _natures[GameController.Instance.Random.Next(0, _natures.Length)];
        }
    }
}
