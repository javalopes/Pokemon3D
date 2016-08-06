using System;
using System.Collections.Generic;
using System.IO;

namespace Assets.Editor
{
    internal class FilePathExporter
    {
        private readonly string _gameModePath;
        private readonly HashSet<string> _exportedFilePaths = new HashSet<string>();

        public FilePathExporter(string gameModePath, bool createMissingFolders)
        {
            _gameModePath = string.IsNullOrEmpty(gameModePath) ? Environment.GetFolderPath(Environment.SpecialFolder.Desktop): gameModePath;

            if (createMissingFolders)
            {
                if (!Directory.Exists(_gameModePath)) Directory.CreateDirectory(_gameModePath);

                var mapPath = GetMapPath();
                if (!Directory.Exists(mapPath)) Directory.CreateDirectory(mapPath);

                var modelsPath = GetModelsPath();
                if (!Directory.Exists(modelsPath)) Directory.CreateDirectory(modelsPath);
            }
        }

        public string GetFolderPathMap(string fileName)
        {
            return Path.Combine(GetMapPath(), fileName);
        }

        public string GetFolderPathModels(string fileName)
        {
            return Path.Combine(GetModelsPath(), fileName);
        }

        public string ExportAssetToModels(string sourceAssetPath)
        {
            var targetFilePath = Path.Combine(GetModelsPath(), Path.GetFileName(sourceAssetPath) ?? "");
            if (!_exportedFilePaths.Contains(sourceAssetPath))
            {
                File.Copy(sourceAssetPath, targetFilePath, true);
                _exportedFilePaths.Add(sourceAssetPath);
            }
            return Path.GetFileName(sourceAssetPath);
        }

        public string ExportAssetToModels(string sourceAssetPath, byte[] data)
        {
            var targetFilePath = GetFolderPathModels((Path.GetFileNameWithoutExtension(sourceAssetPath) ?? "") + ".pmesh");
            if (!_exportedFilePaths.Contains(sourceAssetPath))
            {
                File.WriteAllBytes(targetFilePath, data);
                _exportedFilePaths.Add(sourceAssetPath);
            }
            return Path.GetFileName(targetFilePath);
        }

        private string GetModelsPath()
        {
            return Path.Combine(Path.Combine(_gameModePath, "Content"), "Models");
        }

        private string GetMapPath()
        {
            return Path.Combine(_gameModePath, "Maps");
        }
    }
}
