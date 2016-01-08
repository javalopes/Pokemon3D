using Pokemon3D.Editor.Core.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core.DetailViewModels
{
    public class TextureDetailViewModel : ViewModel
    {
        public string AbsoluteFilePath { get; private set; }
        public string FileName { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public TextureDetailViewModel(string filePath)
        {
            AbsoluteFilePath = filePath;
            FileName = Path.GetFileName(filePath);
            Width = 100;
            Height = 100;
        }
    }
}
