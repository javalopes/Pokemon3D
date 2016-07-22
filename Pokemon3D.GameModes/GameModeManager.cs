using Pokemon3D.Common.FileSystem;
using System.Collections.Generic;
using System.IO;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.Common;

namespace Pokemon3D.Entities
{
    /// <summary>
    /// A class to handle all loaded GameModes.
    /// </summary>
    public class GameModeManager
    {
        /// <summary>
        /// Returns a collection of GameModes information.
        /// </summary>
        public GameModeInfo[] GetGameModeInfos()
        {
            var gameModes = new List<GameModeInfo>();
            foreach (var gameModeDirectory in Directory.GetDirectories(GameModePathProvider.GameModeFolder, "*.*", SearchOption.TopDirectoryOnly))
            {
                gameModes.Add(new GameModeInfo(gameModeDirectory));
            }
            return gameModes.ToArray();
        }

        public GameMode ActiveGameMode { get; set; }

        public GameMode CreateGameMode(GameModeInfo gameModeInfo, GameContext gameContext)
        {
            return new GameMode(gameModeInfo, gameContext, new FileLoader());
        }
    }
}
