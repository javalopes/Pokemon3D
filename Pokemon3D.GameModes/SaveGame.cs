using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.GameModes.Pokemon;

namespace Pokemon3D.GameModes
{
    public class SaveGame
    {
        private SaveFileModel _dataModel;
        
        public SaveGame(SaveFileModel dataModel)
        {
            _dataModel = dataModel;
        }

        public string PlayerName => _dataModel.PlayerData.Name;

        public int Money
        {
            get { return _dataModel.PlayerData.Money; }
            set { _dataModel.PlayerData.Money = value; }
        }
    }
}
