using Pokemon3D.GameModes;
using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A container for data request results to file content.
    /// </summary>
    class DataRequest<T> where T : JsonDataModel<T>
    {
        private GameMode _gameMode;

        /// <summary>
        /// The current status of this data request.
        /// </summary>
        public DataRequestStatus Status { get; private set; }

        /// <summary>
        /// The path the data should be retrieved from.
        /// </summary>
        public string DataPath { get; private set; }

        /// <summary>
        /// The resulting data of the request operation.
        /// </summary>
        public string ResultData { get; private set; }

        /// <summary>
        /// The final model created from the result data.
        /// </summary>
        public T ResultModel { get; private set; }

        /// <summary>
        /// Event that gets fired right when the request started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// After the request finished, this event gets fired.
        /// </summary>
        public event EventHandler Finished;

        /// <summary>
        /// Creates a GameMode data request to a file containing data.
        /// </summary>
        public DataRequest(GameMode gameMode, string dataPath)
        {
            Status = DataRequestStatus.NotStarted;

            _gameMode = gameMode;
            DataPath = dataPath;

            ResultData = string.Empty;
        }

        /// <summary>
        /// Starts the request using threading.
        /// </summary>
        public void StartThreaded()
        {
            Status = DataRequestStatus.Incomplete;

            if (Started != null)
                Started(this, EventArgs.Empty);

            if (true) // check if game is running in server mode or offline mode
            {
                // offline mode
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetDataOffline));
            }
            else
            {
                // server mode
                // nothing so far...
            }
        }

        /// <summary>
        /// Starts the request.
        /// </summary>
        public void Start()
        {
            Status = DataRequestStatus.Incomplete;

            if (Started != null)
                Started(this, EventArgs.Empty);

            if (true) // check if game is running in server mode or offline mode
            {
                // offline mode
                GetDataOffline(null);
            }
            else
            {
                // server mode
                // nothing so far...
            }
        }

        private void GetDataOffline(object o)
        {
            // getting the data offline just means that we read the file and return the contents.
            // first, create the full path:

            string path = Path.Combine(_gameMode.GameModeInfo.DirectoryPath, DataPath);

            // check if the file exists:

            if (File.Exists(path))
            {
                try
                {
                    ResultData = File.ReadAllText(path);
                    ResultModel = JsonDataModel<T>.FromString(ResultData);
                    Status = DataRequestStatus.Complete;
                }
                catch (IOException)
                {
                    Status = DataRequestStatus.Error;
                }
                catch (JsonDataLoadException)
                {
                    Status = DataRequestStatus.Error;
                }
            }
            else
            {
                Status = DataRequestStatus.Error;
            }

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }
    }
}
