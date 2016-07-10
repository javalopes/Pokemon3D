using System.IO;

namespace Pokemon3D.Editor.Core.Model
{
    public abstract class ResourceModel
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string[] HierarchyPath { get; set; }

        public ResourceType ResourceType { get; set; }

        protected ResourceModel(string basePath, string filePath)
        {
            FilePath = filePath;
            var localPath = (Path.GetDirectoryName(filePath) ?? "").Replace(basePath.Trim(), "");
            HierarchyPath = localPath.Split(new[] { Path.DirectorySeparatorChar }, System.StringSplitOptions.RemoveEmptyEntries);
        }
    }
}