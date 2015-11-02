﻿using System.IO;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A file provider for localization files.
    /// </summary>
    class TranslationFileProvider : FileProvider
    {
        const string LOCALIZATION_DIRECTORY = "Localization";

        /// <summary>
        /// The lookup path for localization files.
        /// </summary>
        public static string LookupPath
        {
            get 
            {
                return Path.Combine(new string[] { StartupPath, LOCALIZATION_DIRECTORY });
            }
        }
    }
}