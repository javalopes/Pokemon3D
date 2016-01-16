using Pokemon3D.DataModel.GameMode.Definitions;
using Pokemon3D.GameCore;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Natures loaded from the Natures data file.
    /// </summary>
    class NatureManager
    {
        private GameMode _gameMode;
        private NatureModel[] _natureModels;

        public NatureManager(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        public void PreloadAsync()
        {
            _gameMode.GetFileAsync(_gameMode.NaturesFilePath, OnDataReceived);
        }

        private void OnDataReceived(byte[] data)
        {
            _natureModels = DataModel.DataModel<NatureModel[]>.FromByteArray(data);
            PreloadCompleted = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool PreloadCompleted { get; private set; }

        /// <summary>
        /// Returns the <see cref="NatureModel"/> for a nature id.
        /// </summary>
        public NatureModel GetNatureModel(string id)
        {
            if (id == null) return null;
            return _natureModels.SingleOrDefault(x => x.Id == id);
        }

        public NatureModel GetRandomNature()
        {
            return _natureModels[GameController.Instance.Random.Next(0, _natureModels.Length)];
        }
    }
}
