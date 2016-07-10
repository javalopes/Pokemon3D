using System.IO;

namespace Pokemon3D.Editor.Core.Model
{
    public class TextureModel : ResourceModel
    {
        public TextureModel(string basePath, string filePath) : base(basePath, filePath)
        {
            ResourceType = ResourceType.TextureFile;
            Name = Path.GetFileName(filePath);
        }
    }
}