using System;
using System.Text;
using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    public class LogPathProvider : PathProvider
    {
        private const string LogFileExtension = ".txt";
        const string LogFilePrefix = "log_";
        public const string LogDirectoryName = "Logs";

        /// <summary>
        /// The directory logs are placed in.
        /// </summary>
        public static string LogDirectory => Path.Combine(StartupPath, LogDirectoryName);

        /// <summary>
        /// Returns the path to the current log file.
        /// </summary>
        public static string LogFile
        {
            get
            {
                /*
                  The log file will have this format:
                  log_yyyy-mm-dd.txt, corresponding to when the log file name is requested.
                */

                var now = DateTime.Now;
                StringBuilder sb = new StringBuilder(LogFilePrefix);

                //D2 formats the number with a leading zero, if needed.
                sb.Append(now.Year.ToString());
                sb.Append("-");
                sb.Append(now.Month.ToString("D2"));
                sb.Append("-");
                sb.Append(now.Day.ToString("D2"));
                sb.Append(LogFileExtension); //Append file ending.

                //Combine the startup path and the file name constructed by the string builder:
                return Path.Combine(LogDirectory, sb.ToString());
            }
        }
    }
}
