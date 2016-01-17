using Pokemon3D.Common.DataHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace Pokemon3D.FileSystem
{
    public class FileLoader : AsyncDataLoader, FileProvider
    {
        private readonly Dictionary<string, byte[]> _fileCache;

        public FileLoader()
        {
            _fileCache = new Dictionary<string, byte[]>();
        }

        protected override DataLoadResult OnRequestData(string resourcePath)
        {
            byte[] existing;
            if (_fileCache.TryGetValue(resourcePath, out existing)) return DataLoadResult.Succeeded(existing);

            try
            {
                existing = File.ReadAllBytes(resourcePath);
            }
            catch (Exception ex)
            {
                return DataLoadResult.Failed(ex.Message);
            }
            _fileCache.Add(resourcePath, existing);

            return DataLoadResult.Succeeded(existing);
        }

        public void GetFileAsync(string filePath, Action<DataLoadResult> onDataReceived)
        {
            LoadAsync(filePath, onDataReceived);
        }

        public void GetFilesAsync(string[] filePaths, Action<DataLoadResult[]> onDataReceived)
        {
            LoadAsync(filePaths, onDataReceived);
        }

        public byte[] GetFile(string filePath)
        {
            byte[] data = null;
            var waitForEnded = new AutoResetEvent(false);
            LoadAsync(filePath, r =>
            {
                data = r.Data;
                waitForEnded.Set();
            });

            waitForEnded.WaitOne();

            return data;
        }
    }
}
