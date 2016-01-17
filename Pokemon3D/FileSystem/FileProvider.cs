using System;
using Pokemon3D.Common.DataHandling;

namespace Pokemon3D.FileSystem
{
    public interface FileProvider
    {
        void GetFileAsync(string filePath, Action<DataLoadResult> onDataReceived);

        void GetFilesAsync(string[] filePaths, Action<DataLoadResult[]> onDataReceived);

        void GetFilesOfFolderAsync(string folderPath, Action<DataLoadResult[]> onDataReceived);

        byte[] GetFile(string filePath);
    }
}