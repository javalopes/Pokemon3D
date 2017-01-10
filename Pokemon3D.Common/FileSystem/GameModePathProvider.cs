using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// Provides access to paths related to GameModes.
    /// </summary>
    public class GameModePathProvider : PathProvider
    {
        private const string PathGamemodes = "GameModes";
        private const string FileGamemodeMain = "GameMode.json";

        public static string CustomPath { private get; set; } = null;

        /// <summary>
        /// The path to the base GameMode folder.
        /// </summary>
        public static string GameModeFolder => CustomPath != null ? Path.GetFullPath(CustomPath) : Path.Combine(StartupPath, PathGamemodes);

        /// <summary>
        /// Returns the path to a GameMode config file.
        /// </summary>
        public static string GetGameModeFile(string folder)
        {
            return Path.Combine(new[] { folder, FileGamemodeMain });
        }
    }
}
