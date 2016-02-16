using System;
using Pokemon3D.Editor.Core.Model;

namespace Pokemon3D.Editor.Core.DetailViewModels
{
    public class TextureDetailViewModel : DetailViewModel
    {
        public string AbsoluteFilePath { get; private set; }
        public string FileName { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public TextureDetailViewModel(TextureModel textureModel)
        {
            AbsoluteFilePath = textureModel.FilePath;
            FileName = textureModel.Name;
            Width = 100;
            Height = 100;
        }

        internal override void OnActivate()
        {
        }

        internal override void OnDeactivate()
        {
        }
    }
}
