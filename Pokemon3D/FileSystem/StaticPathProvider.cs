using System.IO;
using Pokemon3D.Common.FileSystem;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// Provides paths to static files of the game.
    /// </summary>
    internal class StaticPathProvider : PathProvider
    {
        private const string ConfigFileName = "configuration.json";
        private const string SaveDirectory = "Saves";

        /// <summary>
        /// The path to the main configuration file of the game.
        /// </summary>
        public static string ConfigFile => Path.Combine(StartupPath, ConfigFileName);

        /// <summary>
        /// The path to the save files.
        /// </summary>
        public static string SavePath => Path.Combine(StartupPath, SaveDirectory);
    }
}