using Pokemon3D.Common.Diagnostics;
using Pokemon3D.Common.FileSystem;
using Pokemon3D.DataModel;
using Pokemon3D.DataModel.GameCore;
using Pokemon3D.FileSystem;
using System;
using System.IO;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Contains the global settings of the game, loaded from the configuration.json file.
    /// </summary>
    internal class GameConfiguration
    {
        public event EventHandler ConfigFileLoaded;

        public ConfigurationModel Data { get; private set; }

        public GameConfiguration()
        {
            if (File.Exists(StaticPathProvider.ConfigFile))
            {
                try
                {
                    Data = DataModel<ConfigurationModel>.FromFile(StaticPathProvider.ConfigFile);
                }
                catch (DataLoadException)
                {
                    GameLogger.Instance.Log(MessageType.Error, "Error trying to load the configuration file of the game. Resetting file.");

                    Data = ConfigurationModel.Default;
                    Save();
                }
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Warning, "Configuration file not found. Creating new one.");

                Data = ConfigurationModel.Default;
                Save();
            }

            GameInstance.Exiting += OnGameExiting;
            GameModePathProvider.CustomPath = Data.CustomGameModeBasePath;

            FileObserver.Instance.StartFileObserve(StaticPathProvider.ConfigFile, ReloadFile);
        }
        
        private void ReloadFile(object sender, FileSystemEventArgs e)
        {
            try
            {
                Data = DataModel<ConfigurationModel>.FromFile(StaticPathProvider.ConfigFile);
                GameModePathProvider.CustomPath = Data.CustomGameModeBasePath;
                ConfigFileLoaded?.Invoke(this, EventArgs.Empty);
            }
            catch (DataLoadException)
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
            Data.ToFile(StaticPathProvider.ConfigFile, DataType.Json);
        }
    }
}
