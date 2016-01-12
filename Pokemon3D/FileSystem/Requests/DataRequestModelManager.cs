using Pokemon3D.DataModel;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;

namespace Pokemon3D.FileSystem.Requests
{
    /// <summary>
    /// Manages models requested and loaded by a <see cref="DataRequest"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data model.</typeparam>
    abstract class DataRequestModelManager<T> where T : DataModel<T>
    {
        private Dictionary<string, T> _modelBuffer;

        protected GameMode _gameMode;

        public DataRequestModelManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _modelBuffer = new Dictionary<string, T>();
        }

        private void FinishedLoadingModel(object sender, EventArgs e)
        {
            var request = (DataModelRequest<T>)sender;
            if (_modelBuffer.ContainsKey(request.DataPath))
            {
                _modelBuffer[request.DataPath] = request.ResultModel;
            }
            else
            {
                _modelBuffer.Add(request.DataPath, request.ResultModel);
            }
        }

        /// <summary>
        /// Creates a new data request for a specific model defined by its path.
        /// </summary>
        public virtual DataModelRequest<T> CreateDataRequest(string dataPath)
        {
            var request = new DataModelRequest<T>(_gameMode, dataPath);
            request.Finished += FinishedLoadingModel;
            return request;
        }

        /// <summary>
        /// Checks if a model from a path is already in the buffer.
        /// </summary>
        public bool HasModelInBuffer(string dataPath)
        {
            return _modelBuffer.ContainsKey(dataPath);
        }

        /// <summary>
        /// Retrieves a model from a path from the buffer.
        /// </summary>
        public T GetModelFromBuffer(string dataPath)
        {
            return _modelBuffer[dataPath];
        }

        /// <summary>
        /// Returns either the request to load a model or the model itself, if it exists in the buffer.
        /// </summary>
        public object GetRequestOrModel(string dataPath)
        {
            if (HasModelInBuffer(dataPath))
                return GetModelFromBuffer(dataPath);
            else
                return CreateDataRequest(dataPath);
        }
    }
}
