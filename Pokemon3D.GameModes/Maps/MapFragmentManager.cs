using System;
using System.Collections.Generic;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.DataModel.GameMode.Map;
using System.IO;

namespace Pokemon3D.GameModes.Maps
{
    public class MapFragmentManager
    {
        private readonly Dictionary<string, MapFragmentModel> _fragmentModelCache;
        private readonly GameMode _gameMode;

        public MapFragmentManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _fragmentModelCache = new Dictionary<string, MapFragmentModel>();
        }

        public void LoadFragmentAsync(string dataPath, Action<MapFragmentModel> fragmentLoaded)
        {
            MapFragmentModel fragment;
            if (_fragmentModelCache.TryGetValue(dataPath, out fragment))
            {
                fragmentLoaded(fragment);
            }

            _gameMode.FileLoader.GetFileAsync(_gameMode.GetMapFragmentFilePath(dataPath), a => OnFragmentLoaded(dataPath, a, fragmentLoaded));
        }

        private void OnFragmentLoaded(string dataPath, DataLoadResult data, Action<MapFragmentModel> fragmentLoaded)
        {
            var fragment = DataModel.DataModel<MapFragmentModel>.FromByteArray(data.Data); 
            _fragmentModelCache.Add(dataPath, fragment);
            fragmentLoaded(fragment);
        }
    }
}
