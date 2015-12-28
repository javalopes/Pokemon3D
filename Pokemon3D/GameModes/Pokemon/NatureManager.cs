using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json;
using Pokemon3D.DataModel.Json.GameMode.Definitions;
using System.IO;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// Manages Natures loaded from the Natures data file.
    /// </summary>
    class NatureManager
    {
        private const string NATURES_FILE_NAME = "Natures.json";

        private GameMode _gameMode;
        private NatureModel[] _natures;
        private bool _loaded = false;
        private Random _randomizer;

        public NatureManager(GameMode gameMode)
        {
            _gameMode = gameMode;
            _randomizer = new Random();
        }

        private void LoadNatures()
        {
            string path = Path.Combine(_gameMode.PokemonDataPath, NATURES_FILE_NAME);

            if (File.Exists(path))
            {
                try
                {
                    _natures = JsonDataModel<NatureModel[]>.FromFile(path);
                }
                catch (JsonDataLoadException ex)
                {
                    GameLogger.Instance.Log(ex);
                }
            }
            else
            {
                GameLogger.Instance.Log(MessageType.Error, NATURES_FILE_NAME + " file not found.");
            }

            _loaded = true;
        }

        /// <summary>
        /// Returns a nature with a given Id.
        /// </summary>
        public NatureModel GetNature(string natureId)
        {
            if (!_loaded)
                LoadNatures();

            return _natures.Single(n => n.Id == natureId);
        }

        /// <summary>
        /// Returns a random nature from all loaded natures.
        /// </summary>
        public NatureModel GetRandomNature()
        {
            if (!_loaded)
                LoadNatures();

            return _natures[_randomizer.Next(0, _natures.Length)];
        }
    }
}
