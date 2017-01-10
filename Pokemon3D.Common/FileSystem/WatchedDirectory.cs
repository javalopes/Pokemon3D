using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    /// <summary>
    /// A directory watched by the <see cref="FileObserver"/> class.
    /// </summary>
    class WatchedDirectory : WatchedResource
    {
        public event WatchedDirectoryChangeEventHandler WatcherEvent;
        private int _handleCount;

        public bool HasHandles => _handleCount > 0;

        public WatchedDirectory(string directoryPath, string fileExtension, WatchedDirectoryChangeEventHandler eventHandler)
            : base(directoryPath)
        {
            Watcher = new FileSystemWatcher(directoryPath, fileExtension);
            Watcher.Changed += OnWatcherEvent;
            Watcher.Created += OnWatcherEvent;
            Watcher.Deleted += OnWatcherEvent;
            Watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            Watcher.EnableRaisingEvents = true;

            AddHandler(eventHandler);
        }

        private void OnWatcherEvent(object sender, FileSystemEventArgs e)
        {
            bool isFile = !File.GetAttributes(e.FullPath).HasFlag(FileAttributes.Directory);

            WatchedDirectoryEventArgs args = new WatchedDirectoryEventArgs(e.ChangeType, isFile, e.FullPath);
            WatcherEvent?.Invoke(this, args);
        }

        public void AddHandler(WatchedDirectoryChangeEventHandler eventHandler)
        {
            WatcherEvent += eventHandler;
            _handleCount++;
        }

        public void RemoveHandler(WatchedDirectoryChangeEventHandler eventHandler)
        {
            WatcherEvent -= eventHandler;
            _handleCount--;
        }
    }
}
