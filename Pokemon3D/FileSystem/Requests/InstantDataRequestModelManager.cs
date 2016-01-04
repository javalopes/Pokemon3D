using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;

namespace Pokemon3D.FileSystem.Requests
{
    /// <summary>
    /// Loads all resources at once instead of on demand through a <see cref="DataRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data model.</typeparam>
    abstract class InstantDataRequestModelManager<T> where T : DataModel<T>
    {
        private DataRequest _request;
        protected T[] _modelBuffer;
        protected GameMode _gameMode;
        protected string _dataPath;
        protected bool _singleModelPerFile; // if the file(s) that get loaded by this manager have a single model in them or an array of models.

        /// <summary>
        /// Getting fired once the loading finished.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// The status of the <see cref="DataRequest"/> of this manager.
        /// </summary>
        public DataRequestStatus Status
        {
            get
            {
                return _request == null ? DataRequestStatus.NotStarted : _request.Status;
            }
        }
        
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
            _request = new DataRequest(_gameMode, _dataPath);
            _request.Finished += FinishedRequest;
            _request.StartThreaded();
        }

        private void FinishedRequest(object sender, EventArgs e)
        {
            List<T> buffer = new List<T>();

            foreach (var result in _request.ResultData)
            {
                if (_singleModelPerFile)
                    buffer.Add(DataModel<T>.FromString(result.FileContent));
                else
                    buffer.AddRange(DataModel<T[]>.FromString(result.FileContent));
            }

            _modelBuffer = buffer.ToArray();

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }
    }
}
