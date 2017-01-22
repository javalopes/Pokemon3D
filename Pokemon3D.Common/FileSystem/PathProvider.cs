using System;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// The base class for path providers.
    /// </summary>
    public class PathProvider
    {
        /// <summary>
        /// Returns the start up path of the executable.
        /// </summary>
        protected static string StartupPath => AppDomain.CurrentDomain.BaseDirectory;
    }
}
