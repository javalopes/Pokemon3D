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

        protected override byte[] OnRequestData(string resourcePath)
        {
            byte[] existing;
            if (_fileCache.TryGetValue(resourcePath, out existing)) return existing;

            existing = File.ReadAllBytes(resourcePath);
            _fileCache.Add(resourcePath, existing);

            return existing;
        }

        public void GetFileAsync(string filePath, Action<byte[]> onDataReceived)
        {
            LoadAsync(filePath, r => onDataReceived(r.Data));
        }

        public void GetFilesAsync(string[] filePaths, Action<byte[][]> onDataReceived)
        {
            LoadAsync(filePaths, r => onDataReceived(r.Select(d => d.Data).ToArray()));
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
