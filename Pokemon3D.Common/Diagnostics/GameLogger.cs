using System;
using System.Diagnostics;
using System.IO;
using Pokemon3D.Common.FileSystem;

namespace Pokemon3D.Common.Diagnostics
{
    /// <summary>
    /// A class to log events during the game's runtime.
    /// </summary>
    public class GameLogger : Singleton<GameLogger>
    {
        private const string LoggerFileLineFormat = "{0} {1} {2}\r\n";
        private const string LoggerVsFormat = "{0} {1} {2}\r\n";
        private const string ExceptionMessageFormat = "An exception occurred! Message: {0}; Type: {1}; Other details: {2}\r\n";

        private string _logFilePath;

        private GameLogger()
        {
            Initialize();
        }

        private void EnsureFolderExists()
        {
            var directory = Path.GetDirectoryName(_logFilePath) ?? "";
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        }

        /// <summary>
        /// Initializes the logger.
        /// </summary>
        public void Initialize()
        {
            _logFilePath = LogPathProvider.LogFile;
            EnsureFolderExists();
        }

        /// <summary>
        /// Stores an entry in the game's log file.
        /// </summary>
        public void Log(MessageType messageType, string message)
        {
            var icon = GetMessageTypeIcon(messageType);

            if (messageType != MessageType.Debug)
            {
                var outLine = string.Format(LoggerFileLineFormat, DateTime.Now.ToLongTimeString(), icon, message);
                File.AppendAllText(_logFilePath, outLine);
            }

#if DEBUG
            if (Debugger.IsAttached) Debug.Print(LoggerVsFormat, DateTime.Now.ToLongTimeString(), icon, message);
            else Console.WriteLine(LoggerVsFormat, DateTime.Now.ToLongTimeString(), icon, message);
#endif
        }

        /// <summary>
        /// Stores an exception message in the game's log file.
        /// </summary>
        public void Log(Exception ex)
        {
            Log(MessageType.Error, string.Format(ExceptionMessageFormat, ex.Message, ex.GetType().Name, ""));

            if (ex.InnerException != null)
                Log(ex.InnerException);
        }

        /// <summary>
        /// Returns an ASCII icon for a message type.
        /// </summary>
        private string GetMessageTypeIcon(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Debug:
                    return "|>|";
                case MessageType.Message:
                    return "(!)";
                case MessageType.Warning:
                    return @"/!\";
                case MessageType.Error:
                    return "(X)";
                default:
                    return string.Empty;
            }
        }
    }
}