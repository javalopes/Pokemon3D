using Pokemon3D.DataModel.Json.GameMode.Map;
using Pokemon3D.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Maps
{
    class MapFragmentManager : DataRequestModelManager<MapFragmentModel>
    {
        public MapFragmentManager(GameMode gameMode) : base(gameMode)
        { }

        private string CreateKey(string dataPath)
        {
            return "Fragments\\" + dataPath + ".json";
        }

        public override DataRequest<MapFragmentModel> CreateDataRequest(string dataPath)
        {
            return base.CreateDataRequest(CreateKey(dataPath));
        }
    }
}
