using Pokemon3D.Common.Diagnostics;
using Pokemon3D.DataModel.Json;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A container for data request results to file content.
    /// </summary>
    class DataRequest
    {
        protected GameMode _gameMode;

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
        /// Event that gets fired right when the request started.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// After the request finished, this event gets fired.
        /// </summary>
        public event EventHandler Finished;

        private Dictionary<string, object> _dataContext;

        /// <summary>
        /// Data context that holds customizable data entires.
        /// </summary>
        public Dictionary<string, object> DataContext
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new Dictionary<string, object>();

                return _dataContext;
            }
        }

        /// <summary>
        /// An exception that occurred during the request. If no exception occurred, this value is equal to null.
        /// </summary>
        public Exception RequestException { get; private set; }

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

            GameLogger.Instance.Log(MessageType.Debug, "Data Request for data path \"" + DataPath + "\" started (async).");

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

            GameLogger.Instance.Log(MessageType.Debug, "Data Request for data path \"" + DataPath + "\" started.");

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
                    Status = DataRequestStatus.Complete;
                }
                catch (IOException ex)
                {
                    RequestException = new DataRequestException(this, DataRequestErrorType.FileReadError, ex);
                    Status = DataRequestStatus.Error;
                }
                catch (JsonDataLoadException ex)
                {
                    RequestException = new DataRequestException(this, DataRequestErrorType.JsonDataError, ex);
                    Status = DataRequestStatus.Error;
                }
                catch (Exception ex)
                {
                    RequestException = new DataRequestException(this, DataRequestErrorType.MiscError, ex);
                    Status = DataRequestStatus.Error;
                }
            }
            else
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.FileNotFound, null);
                Status = DataRequestStatus.Error;
            }

            GameLogger.Instance.Log(MessageType.Debug, "Data Request for data path \"" + DataPath + "\" finished  (Status: " + Status.ToString() + ").");

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }
    }
}
