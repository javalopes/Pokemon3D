using System;
using Pokemon3D.DataModel.GameMode.Map;

namespace Pokemon3D.GameModes.Maps
{
    class MapManager
    {
        private readonly GameMode _gameMode;

        public MapManager(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        public void LoadMapAsync(string dataPath, Action<MapModel> mapModelLoaded)
        {
            _gameMode.FileLoader.GetFileAsync(_gameMode.GetMapFilePath(dataPath), a => OnMapLoaded(a, mapModelLoaded));
        }

        private void OnMapLoaded(byte[] bytes, Action<MapModel> mapModelLoaded)
        {
            mapModelLoaded(DataModel.DataModel<MapModel>.FromByteArray(bytes));
        }
    }
}
