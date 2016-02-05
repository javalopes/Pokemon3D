using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.Model
{
    static class FileSystem
    {
        public static void GetFilesRecursive(string folderPath, Action<string> onFileFound)
        {
            foreach (var directory in Directory.GetDirectories(folderPath))
            {
                GetFilesRecursive(directory, onFileFound);
            }

            GetFiles(folderPath, onFileFound);
        }

        public static void GetFiles(string folderPath, Action<string> onFileFound)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                onFileFound(file);
            }
        }

        public static void GetFilesOfFolderRecursive(string folderPath, Action<string[]> onFilesInFolderFound)
        {
            foreach (var directory in Directory.GetDirectories(folderPath))
            {
                GetFilesOfFolderRecursive(directory, onFilesInFolderFound);
            }

            var filesInCurrentFolder = Directory.GetFiles(folderPath);
            if (filesInCurrentFolder.Length > 0) onFilesInFolderFound(filesInCurrentFolder);
        }

        public static void EnsureFolderExists(string folderPath)
        {
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        }
    }
}
