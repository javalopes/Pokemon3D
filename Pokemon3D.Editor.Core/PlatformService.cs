using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Editor.Core
{
    public interface PlatformService
    {
        string ShowSelectFolderDialog();

        void Activate3DViewForModel(string filePath);

        void Deactivate3DView();
    }
}
