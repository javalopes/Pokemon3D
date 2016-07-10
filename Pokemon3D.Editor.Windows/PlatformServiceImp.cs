using Pokemon3D.Editor.Core;
using System;

namespace Pokemon3D.Editor.Windows
{
    class PlatformServiceImp : PlatformService
    {
        public Action<string> OnErrorOccurred
        {
            get; set;
        }

        public void Activate3DViewForModel(string filePath)
        {
           
        }

        public void Deactivate3DView()
        {
        }

        public string ShowSelectFolderDialog()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return null;
        }
    }
}
