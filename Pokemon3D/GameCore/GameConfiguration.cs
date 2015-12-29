using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameCore;
using Pokemon3D.FileSystem;
using System.IO;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Common.FileSystem;
using System;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Contains the global settings of the game, loaded from the configuration.json file.
    /// </summary>
    class GameConfiguration : GameObject
    {
        private ConfigurationModel _dataModel;

        public event EventHandler ConfigFileLoaded;

        public string DisplayLanguage => _dataModel.DisplayLanguage;
        public SizeModel WindowSize => _dataModel.WindowSize;

        public GameConfiguration()
        {
            if (File.Exists(StaticFileProvider.ConfigFile))
            {
                try
                {
                    _dataModel = DataModel<ConfigurationModel>.FromFile(StaticFileProvider.ConfigFile);
                }
                catch (JsonDataLoadException)
                {
                    GameLogger.Instance.Log(MessageType.Error, "Error trying to load the configuration file of the game. Resetting file.");

                    _dataModel = ConfigurationModel.Default;
                    Save();
                }
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Warning, "Configuration file not found. Creating new one.");

                _dataModel = ConfigurationModel.Default;
                Save();
            }

            Game.Exiting += OnGameExiting;

            FileObserver.Instance.StartFileObserve(StaticFileProvider.ConfigFile, ReloadFile);
        }

        public bool ShadowsEnabled
        {
            get { return _dataModel.ShadowsEnabled; }
            set { _dataModel.ShadowsEnabled = value; }
        }

        public bool SoftShadows
        {
            get { return _dataModel.SoftShadows; }
            set { _dataModel.SoftShadows = value; }
        }

        public ShadowQuality ShadowQuality
        {
            get { return _dataModel.ShadowQuality; }
            set { _dataModel.ShadowQuality = value; }
        }

        private void ReloadFile(object sender, FileSystemEventArgs e)
        {
            try
            {
                _dataModel = DataModel<ConfigurationModel>.FromFile(StaticFileProvider.ConfigFile);
                ConfigFileLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (JsonDataLoadException)
            {
                GameLogger.Instance.Log(MessageType.Error, "Error trying to load the configuration file of the game.");
            }
            catch (IOException)
            {
                GameLogger.Instance.Log(MessageType.Error, "Failed to access the contents of the file.");
            }
        }

        private void OnGameExiting(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            GameLogger.Instance.Log(MessageType.Message, "Saving configuration file.");
            _dataModel.ToFile(StaticFileProvider.ConfigFile);
        }
    }
}
