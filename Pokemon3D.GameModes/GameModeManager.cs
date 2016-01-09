using Pokemon3D.Common.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes
{
    public class GameModeInfoManager
    {
        public GameModeInfo GetGameMode(string folderName)
        {
            return new GameModeInfo(Path.Combine(GameModePathProvider.GameModeFolder, folderName));
        }
    }
}
