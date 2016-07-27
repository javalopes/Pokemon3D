using System;
using System.Collections.Generic;
using Pokemon3D.Common.DataHandling;
using Pokemon3D.DataModel.GameMode.Map;
using System.IO;

namespace Pokemon3D.Entities.Maps
{
    public class MapFragmentManager
    {
        private static readonly object _lockObject = new object();
        private readonly Dictionary<string, MapFragmentModel> _fragmentModelCache;
        private readonly GameMode _gameMode;

        public MapFragmentManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _fragmentModelCache = new Dictionary<string, MapFragmentModel>();
        }

        public MapFragmentModel GetFragment(string dataPath)
        {
            lock (_lockObject)
            {
                MapFragmentModel fragment;
                if (_fragmentModelCache.TryGetValue(dataPath, out fragment))
                {
                    return fragment;
                }

                var data = _gameMode.FileLoader.GetFile(_gameMode.GetMapFragmentFilePath(dataPath), false);
                fragment = DataModel.DataModel<MapFragmentModel>.FromByteArray(data.Data);
                _fragmentModelCache.Add(dataPath, fragment);
                return fragment;
            }
        }
    }
}
