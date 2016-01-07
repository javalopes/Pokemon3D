using Pokemon3D.FileSystem;
using System.Collections.Generic;
using System.IO;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// A class to handle all loaded GameModes.
    /// </summary>
    class GameModeManager
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

        public GameMode LoadGameMode(GameModeInfo gameModeInfo)
        {
            return new GameMode(gameModeInfo);
        }
    }
}
