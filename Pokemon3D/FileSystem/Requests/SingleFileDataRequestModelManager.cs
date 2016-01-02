using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;
using System;

namespace Pokemon3D.FileSystem.Requests
{
    /// <summary>
    /// Manages multiple objects defined in a single file that are accessed through a <see cref="DataRequest"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class SingleFileDataRequestModelManager<T> where T : DataModel<T>
    {
        protected T[] _modelBuffer;
        protected GameMode _gameMode;
        protected string _dataPath;

        /// <summary>
        /// Getting fired once the loading finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// If the data request for this manager has finished.
        /// </summary>
        public bool FinishedLoading { get; private set; } = false;

        public SingleFileDataRequestModelManager(GameMode gameMode, string dataPath)
        {
            _gameMode = gameMode;
            _dataPath = dataPath;
        }

        /// <summary>
        /// Starts to request the content for this data manager.
        /// </summary>
        public void Start()
        {
            var request = new DataRequest(_gameMode, _dataPath);
            request.Finished += FinishedRequest;
            request.StartThreaded();
        }

        private void FinishedRequest(object sender, EventArgs e)
        {
            var request = (DataRequest)sender;
            _modelBuffer = DataModel<T[]>.FromString(request.ResultData);
            FinishedLoading = true;

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }
    }
}
