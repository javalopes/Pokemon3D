using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// Provides access to paths related to GameModes.
    /// </summary>
    public class GameModePathProvider : PathProvider
    {
        private const string PATH_GAMEMODES = "GameModes";
        private const string FILE_GAMEMODE_MAIN = "GameMode.json";

        public static string CustomPath { private get; set; } = null;

        /// <summary>
        /// The path to the base GameMode folder.
        /// </summary>
        public static string GameModeFolder => CustomPath != null ? Path.GetFullPath(CustomPath) : Path.Combine(StartupPath, PATH_GAMEMODES);

        /// <summary>
        /// Returns the path to a GameMode config file.
        /// </summary>
        public static string GetGameModeFile(string folder)
        {
            return Path.Combine(new[] { folder, FILE_GAMEMODE_MAIN });
        }
    }
}
