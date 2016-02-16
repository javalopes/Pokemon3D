using Pokemon3D.Editor.Core;
using Pokemon3D.Editor.Windows.View3D;
using System;

namespace Pokemon3D.Editor.Windows
{
    class PlatformServiceImp : PlatformService
    {
        private D3D11Host _host;

        public PlatformServiceImp(D3D11Host host)
        {
            if (host == null) throw new ArgumentNullException(nameof(host));
            _host = host;
        }

        public void Activate3DViewForModel(string filePath)
        {
            _host.Activate3DModel(filePath);
        }

        public void Deactivate3DView()
        {
            _host.Deactivate3D();
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
