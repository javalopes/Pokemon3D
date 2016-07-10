using System;
using System.IO;
using System.Linq;

namespace Pokemon3D.Editor.Core.Model
{
    public class ModelModel : ResourceModel
    {
        public ModelModel(string basePath, string[] filePartPaths) 
            : base(basePath, filePartPaths.First(f => f.EndsWith(".obj", StringComparison.OrdinalIgnoreCase)))
        {
            ResourceType = ResourceType.Model;
            Name = Path.GetFileNameWithoutExtension(FilePath);
            HierarchyPath = HierarchyPath.Take(HierarchyPath.Length - 1).ToArray();
        }
    }
}