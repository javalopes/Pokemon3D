using System;

namespace Pokemon3D.FileSystem
{
    public interface FileProvider
    {
        void GetFileAsync(string filePath, Action<byte[]> onDataReceived);

        void GetFilesAsync(string[] filePaths, Action<byte[][]> onDataReceived);

        byte[] GetFile(string filePath);
    }
}