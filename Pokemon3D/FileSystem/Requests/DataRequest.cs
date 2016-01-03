using Pokemon3D.Common.Diagnostics;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.Requests;
using Pokemon3D.GameModes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace Pokemon3D.FileSystem.Requests
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
        public FileContentModel[] ResultData { get; private set; }

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
        }

        // TEMP! Move to somewhere meaningful (and remove const, obviously) once implemented.
        private static bool SERVER_MODE = false;
        private const string SERVER_PATH = "localhost";
        private const string SERVER_PORT = "8080";
        private const string SERVER_API = "api";

        /// <summary>
        /// Starts the request using threading.
        /// </summary>
        public void StartThreaded()
        {
            Status = DataRequestStatus.Incomplete;

            if (Started != null)
                Started(this, EventArgs.Empty);

            GameLogger.Instance.Log(MessageType.Debug, "Data Request for data path \"" + DataPath + "\" started (async).");

            if (!SERVER_MODE) // check if game is running in server mode or offline mode
            {
                // offline mode
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetDataOffline));
            }
            else
            {
                // server mode
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetDataServer));
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

            if (!SERVER_MODE) // check if game is running in server mode or offline mode
            {
                // offline mode
                GetDataOffline(null);
            }
            else
            {
                // server mode
                GetDataServer(null);
            }
        }

        private void GetDataServer(object o)
        {
            string serverPart = string.Format("http://{0}:{1}/{2}", SERVER_PATH, SERVER_PORT, SERVER_API);
            string serverPath = Path.Combine(serverPart, DataPath).Replace("\\", "/");

            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(serverPath);
                request.Method = "GET";
                var response = request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                ResultData = DataModel<FileContentModel[]>.FromString(reader.ReadToEnd());
                Status = DataRequestStatus.Complete;
            }
            catch (JsonDataLoadException ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.JsonDataError, ex);
                Status = DataRequestStatus.Error;
            }
            catch (WebException ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.ServerError, ex);
                Status = DataRequestStatus.Error;
            }
            catch (Exception ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.MiscError, ex);
                Status = DataRequestStatus.Error;
            }

            GameLogger.Instance.Log(MessageType.Debug, "Data Request for data path \"" + DataPath + "\" finished  (Status: " + Status.ToString() + ").");

            if (Finished != null)
                Finished(this, EventArgs.Empty);
        }

        private void GetDataOffline(object o)
        {
            // getting the data offline just means that we read the file and return the contents.
            // first, create the full path:

            string path = Path.Combine(_gameMode.GameModeInfo.DirectoryPath, DataPath);

            if (Directory.Exists(path))
            {
                GetFolderContentOffline(path);
            }
            else if (File.Exists(path))
            {
                GetSingleFileOffline(path);
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

        private void GetSingleFileOffline(string path)
        {
            try
            {
                ResultData = new FileContentModel[] {
                    new FileContentModel() {
                        FileName = path,
                        FileContent = File.ReadAllText(path)
                    }
                };

                Status = DataRequestStatus.Complete;
            }
            catch (IOException ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.FileReadError, ex);
                Status = DataRequestStatus.Error;
            }
            catch (Exception ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.MiscError, ex);
                Status = DataRequestStatus.Error;
            }
        }

        private void GetFolderContentOffline(string path)
        {
            try
            {
                ResultData = Directory.GetFiles(path).Select(file => new FileContentModel()
                {
                    FileName = file,
                    FileContent = File.ReadAllText(file)
                }).ToArray();
                
                Status = DataRequestStatus.Complete;
            }
            catch (IOException ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.FileReadError, ex);
                Status = DataRequestStatus.Error;
            }
            catch (Exception ex)
            {
                RequestException = new DataRequestException(this, DataRequestErrorType.MiscError, ex);
                Status = DataRequestStatus.Error;
            }
        }
    }
}
