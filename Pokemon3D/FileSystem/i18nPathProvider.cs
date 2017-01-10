using Pokemon3D.Common.FileSystem;
using System.IO;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// A file provider for localization files.
    /// </summary>
    internal class I18NPathProvider : PathProvider
    {
        private const string I18NDirectory = "i18n";
        
        /// <summary>
        /// The lookup path for localization files.
        /// </summary>
        public static string LookupPath => Path.Combine(new[] { StartupPath, I18NDirectory });
    }
}
