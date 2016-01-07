using System;
using System.IO;
using System.Text;
using Pokemon3D.Common.FileSystem;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// Provides paths to static files of the game.
    /// </summary>
    class StaticPathProvider : PathProvider
    {
        #region Configuration

        private const string CONFIG_FILE_NAME = "configuration.json";

        /// <summary>
        /// The path to the main configuration file of the game.
        /// </summary>
        public static string ConfigFile
        {
            get
            {
                return Path.Combine(StartupPath, CONFIG_FILE_NAME);
            }
        }

        #endregion
    }
}