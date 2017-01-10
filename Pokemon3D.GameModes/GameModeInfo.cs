﻿using Pokemon3D.DataModel.GameMode;
using Pokemon3D.Common.FileSystem;
using System.IO;
using Pokemon3D.DataModel;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Contains global information about the GameMode.
    /// </summary>
    public class GameModeInfo
    {
        private readonly GameModeModel _gameModeModel;

        public GameModeInfo(string directory)
        {
            DirectioryName = Path.GetFileName(directory);
            DirectoryPath = Path.Combine(GameModePathProvider.GameModeFolder, DirectioryName ?? "");

            IsValid = false;
            string gameModeFile = GameModePathProvider.GetGameModeFile(directory);

            if (File.Exists(gameModeFile))
            {
                try
                {
                    _gameModeModel = DataModel<GameModeModel>.FromFile(gameModeFile);
                    IsValid = true;
                }
                catch (DataLoadException ex)
                {
                    GameLogger.Instance.Log(MessageType.Error, $"An error occurred trying to load the GameMode config file \"{gameModeFile}\".");
                    GameLogger.Instance.Log(MessageType.Error, ex.ToString());
                }
            }
        }

        /// <summary>
        /// The path to this GameMode's folder.
        /// </summary>
        public string DirectoryPath { get; private set; }

        /// <summary>
        /// The name of this GameMode's folder (not full path).
        /// </summary>
        public string DirectioryName { get; }

        /// <summary>
        /// Name of the Game Mode.
        /// </summary>
        public string Name => _gameModeModel?.Name;

        /// <summary>
        /// Author of the Game Mode.
        /// </summary>
        public string Author => _gameModeModel?.Author;

        /// <summary>
        /// Description of Game Mode.
        /// </summary>
        public string Description => _gameModeModel?.Description;

        /// <summary>
        /// Version of Game Mode.
        /// </summary>
        public string Version => _gameModeModel?.Version;

        /// <summary>
        /// The path (relative to \Maps\, without file extension) to the starting map of this GameMode.
        /// </summary>
        public string StartMap => _gameModeModel?.StartConfiguration.Map;

        /// <summary>
        /// The path (relative to \Scripts\, without file extension) to the starting script of this GameMode.
        /// </summary>
        public string StartScript => _gameModeModel?.StartConfiguration.Script;

        /// <summary>
        /// Is this Game Mode valid?
        /// </summary>
        public bool IsValid { get; private set; }
    }
}
