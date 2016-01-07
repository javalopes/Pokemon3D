using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected static string StartupPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
