using System.Collections.Generic;
using Pokemon3D.DataModel.GameMode.Map;

namespace Pokemon3D.GameModes.Maps
{
    public class MapFragmentManager
    {
        private static readonly object LockObject = new object();
        private readonly Dictionary<string, MapFragmentModel> _fragmentModelCache;
        private readonly GameMode _gameMode;

        public MapFragmentManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _fragmentModelCache = new Dictionary<string, MapFragmentModel>();
        }

        public MapFragmentModel GetFragment(string dataPath)
        {
            lock (LockObject)
            {
                MapFragmentModel fragment;
                if (_fragmentModelCache.TryGetValue(dataPath, out fragment))
                {
                    return fragment;
                }

                var data = _gameMode.FileLoader.GetFile(_gameMode.GetMapFragmentFilePath(dataPath));
                fragment = DataModel.DataModel<MapFragmentModel>.FromByteArray(data.Data);
                _fragmentModelCache.Add(dataPath, fragment);
                return fragment;
            }
        }
    }
}
