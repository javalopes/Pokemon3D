using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// Manages models requested and loaded by a <see cref="DataRequest{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data model.</typeparam>
    abstract class DataRequestModelManager<T> where T : DataModel<T>
    {
        private Dictionary<string, T> _modelBuffer;

        private void FinishedLoadingModel(object sender, EventArgs e)
        {
            var request = (DataRequest<T>)sender;
            if (_modelBuffer.ContainsKey(request.DataPath))
            {
                _modelBuffer[request.DataPath] = request.ResultModel;
            }
            else
            {
                _modelBuffer.Add(request.DataPath, request.ResultModel);
            }
        }

        protected GameMode _gameMode;

        /// <summary>
        /// Creates a new data request for a specific model defined by its path.
        /// </summary>
        public virtual DataRequest<T> CreateDataRequest(string dataPath)
        {
            var request = new DataRequest<T>(_gameMode, dataPath);
            request.Finished += FinishedLoadingModel;
            return request;
        }

        public DataRequestModelManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _modelBuffer = new Dictionary<string, T>();
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
    }
}
