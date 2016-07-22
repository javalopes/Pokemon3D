using System;
using Pokemon3D.Common.DataHandling;

namespace Pokemon3D.Common.DataHandling
{
    public interface FileProvider
    {
        DataLoadResult GetFile(string filePath);

        DataLoadResult[] GetFiles(string[] filePaths);

        DataLoadResult[] GetFilesOfFolder(string folderPath);
    }
}