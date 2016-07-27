using Pokemon3D.Common.DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Pokemon3D.Common.DataHandling
{
    public class FileLoader : FileProvider
    {
        private readonly object _lockObject = new object();
        private readonly Dictionary<string, byte[]> _fileCache;

        public FileLoader()
        {
            _fileCache = new Dictionary<string, byte[]>();
        }
        
        public DataLoadResult GetFile(string filePath, bool forceReloadCached)
        {
            lock (_lockObject)
            {
                byte[] existing;
                if (forceReloadCached) _fileCache.Remove(filePath);

                if (_fileCache.TryGetValue(filePath, out existing)) return DataLoadResult.Succeeded(existing);

                try
                {
                    existing = File.ReadAllBytes(filePath);
                }
                catch (Exception ex)
                {
                    return DataLoadResult.Failed(ex.Message);
                }
                _fileCache.Add(filePath, existing);

                return DataLoadResult.Succeeded(existing);
            }
        }

        public DataLoadResult[] GetFiles(string[] filePaths)
        {
            lock (_lockObject)
            {
                var requestResults = new List<DataLoadResult>();
                foreach (var filePath in filePaths)
                {
                    requestResults.Add(GetFile(filePath, false));
                }
                return requestResults.ToArray();
            }
        }

        public DataLoadResult[] GetFilesOfFolder(string folderPath)
        {
            lock (_lockObject)
            {
                var requestResults = new List<DataLoadResult>();
                if (Directory.Exists(folderPath))
                {
                    foreach (var filePath in Directory.GetFiles(folderPath))
                    {
                        requestResults.Add(GetFile(filePath, false));
                    }
                }
                return requestResults.ToArray();
            }
        }
    }
}
