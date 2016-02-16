using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Savegame;
using Pokemon3D.GameModes.Pokemon;
using Pokemon3D.DataModel.Savegame.Pokemon;

namespace Pokemon3D.GameModes
{
    /// <summary>
    /// Exposes the SaveFileModel properties to the API.
    /// </summary>
    public class SaveGame
    {
        private SaveFileModel _dataModel;
        private GameMode _gameMode;

        public SaveGame(SaveFileModel dataModel)
        {
            _dataModel = dataModel;
            PartyPokemon = new List<Pokemon.Pokemon>();
        }

        public string PlayerName => _dataModel.PlayerData.Name;

        public int Money
        {
            get { return _dataModel.PlayerData.Money; }
            set { _dataModel.PlayerData.Money = value; }
        }
        
        public List<Pokemon.Pokemon> PartyPokemon { get; private set; }

        public PokedexSaveModel[] Pokedexes => _dataModel.Pokedexes;

        private int _loadedItems = 0;
        private int _loadingCounter = 0;

        public bool FinishedLoading { get { return _loadedItems == _loadingCounter; } }

        /// <summary>
        /// Loads required data once the <see cref="GameMode"/> is loaded.
        /// </summary>
        /// <param name="gameMode">The GameMode that this save game is linked to.</param>
        public void Load(GameMode gameMode)
        {
            _gameMode = gameMode;

            _loadedItems = 0;
            _loadingCounter = 0;

            if (_dataModel.Pokemon == null)
                _dataModel.Pokemon = new PokemonSaveModel[] { };
            
            _loadingCounter += _dataModel.Pokemon.Length;

            foreach (var pokemon in _dataModel.Pokemon)
            {
                _gameMode.PokemonFactory.GetPokemon(pokemon, (p) =>
                {
                    _loadedItems++;
                    PartyPokemon.Add(p);
                });
            }
        }
    }
}
