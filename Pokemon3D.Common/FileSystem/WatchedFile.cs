using System;
using System.IO;
using System.Threading;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// A file watched by the <see cref="FileObserver"/> class.
    /// </summary>
    class WatchedFile : WatchedResource
    {
        private DateTime _lastRead = DateTime.MinValue;
        public event FileSystemEventHandler WatcherEvent;
        private int _handleCount;

        public bool HasHandles => _handleCount > 0;

        public WatchedFile(string filePath, FileSystemEventHandler eventHandler)
            : base(filePath)
        {
            string fileName = Path.GetFileName(ResourcePath) ?? "";
            string directoryPath = Path.GetDirectoryName(ResourcePath) ?? "";

            Watcher = new FileSystemWatcher(directoryPath, fileName);
            Watcher.Changed += OnWatcherEvent;
            Watcher.NotifyFilter = NotifyFilters.LastWrite;
            Watcher.EnableRaisingEvents = true;

            AddHandler(eventHandler);
        }

        private void OnWatcherEvent(object sender, FileSystemEventArgs e)
        {
            DateTime lastWriteTime = File.GetLastWriteTime(ResourcePath);

            if (lastWriteTime != _lastRead)
            {
                _lastRead = lastWriteTime;

                Thread.Sleep(300);

                Watcher.EnableRaisingEvents = false;
                WatcherEvent?.Invoke(this, e);
                Watcher.EnableRaisingEvents = true;
            }
        }

        public void AddHandler(FileSystemEventHandler eventHandler)
        {
            WatcherEvent += eventHandler;
            _handleCount++;
        }

        public void RemoveHandler(FileSystemEventHandler eventHandler)
        {
            WatcherEvent -= eventHandler;
            _handleCount--;
        }
    }
}
