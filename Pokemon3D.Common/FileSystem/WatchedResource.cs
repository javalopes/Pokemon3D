using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    abstract class WatchedResource
    {
        public string ResourcePath { get; protected set; }
        
        protected FileSystemWatcher Watcher;

        protected WatchedResource(string resourcePath)
        {
            ResourcePath = resourcePath;
        }
    }
}
