using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.DataModel.Json.GameMode.Map;
using System;
using System.IO;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.DataModel.Json;
using Pokemon3D.FileSystem;

namespace Pokemon3D.GameModes.Maps
{
    class MapManager : DataRequestModelManager<MapModel>
    {
        public MapManager(GameMode gameMode) : base(gameMode)
        { }

        private string CreateKey(string dataPath)
        {
            return "Maps\\" + dataPath + ".json";
        }

        public override DataRequest<MapModel> CreateDataRequest(string dataPath)
        {
            return base.CreateDataRequest(CreateKey(dataPath));
        }
    }
}
