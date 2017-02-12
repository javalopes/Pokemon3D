namespace Pokemon3D.Common.DataHandling
{
    public interface IFileProvider
    {
        DataLoadResult GetFile(string filePath, bool forceReloadCached = false);

        DataLoadResult[] GetFiles(string[] filePaths);

        DataLoadResult[] GetFilesOfFolder(string folderPath);
    }
}