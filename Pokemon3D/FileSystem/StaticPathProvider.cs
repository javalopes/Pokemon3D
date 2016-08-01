using System;
using System.IO;
using System.Text;
using Pokemon3D.Common.FileSystem;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// Provides paths to static files of the game.
    /// </summary>
    internal class StaticPathProvider : PathProvider
    {
        private const string CONFIG_FILE_NAME = "configuration.json";
        private const string SAVE_PATH = "Saves";

        /// <summary>
        /// The path to the main configuration file of the game.
        /// </summary>
        public static string ConfigFile => Path.Combine(StartupPath, CONFIG_FILE_NAME);

        /// <summary>
        /// The path to the save files.
        /// </summary>
        public static string SavePath => Path.Combine(StartupPath, SAVE_PATH);
    }
}