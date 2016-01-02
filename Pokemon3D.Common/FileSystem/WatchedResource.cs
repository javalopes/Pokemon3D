using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    abstract class WatchedResource
    {
        public string ResourcePath { get; protected set; }
        
        protected FileSystemWatcher _watcher;

        public WatchedResource(string resourcePath)
        {
            ResourcePath = resourcePath;
        }
    }
}
