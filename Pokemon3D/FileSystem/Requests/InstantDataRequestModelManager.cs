using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;

namespace Pokemon3D.FileSystem.Requests
{
    /// <summary>
    /// Manages multiple objects defined in a single file that are accessed through a <see cref="DataRequest"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract class InstantDataRequestModelManager<T> where T : DataModel<T>
    {
        protected T[] _modelBuffer;
        protected GameMode _gameMode;
        protected string _dataPath;
        protected bool _singleModelPerFile; // if the file(s) that get loaded by this manager have a single model in them or an array of models.

        /// <summary>
        /// Getting fired once the loading finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// If the data request for this manager has finished.
        /// </summary>
        public bool FinishedLoading { get; private set; } = false;
        
        public InstantDataRequestModelManager(GameMode gameMode, string dataPath, bool singleModelPerFile = false)
        {
            _gameMode = gameMode;
            _dataPath = dataPath;
            _singleModelPerFile = singleModelPerFile;
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

            List<T> buffer = new List<T>();

            foreach (var result in request.ResultData)
            {
                if (_singleModelPerFile)
                    buffer.Add(DataModel<T>.FromString(result.FileContent));
                else
                    buffer.AddRange(DataModel<T[]>.FromString(result.FileContent));
            }

            _modelBuffer = buffer.ToArray();
            FinishedLoading = true;

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }
    }
}
