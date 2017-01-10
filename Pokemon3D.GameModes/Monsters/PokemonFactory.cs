﻿using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.DataModel.Pokemon;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel;

namespace Pokemon3D.GameModes.Monsters
{
    /// <summary>
    /// Used to create <see cref="Pokemon"/> instances.
    /// </summary>
    public class PokemonFactory
    {
        private readonly Random _random = new Random();
        private readonly Dictionary<string, PokemonModel> _pokemonModelCache = new Dictionary<string, PokemonModel>();
        private readonly GameMode _gameMode;

        private int[] _charCodes;

        private const int PersonalityValueLength = 10;

        public PokemonFactory(GameMode gameMode)
        {
            _gameMode = gameMode;
        }
        
        public Pokemon GetPokemon(string pokemonId, int level)
        {
            PokemonModel dataModel;

            if (!_pokemonModelCache.TryGetValue(pokemonId, out dataModel))
            {
                var d = _gameMode.FileLoader.GetFile(_gameMode.GetPokemonFilePath(pokemonId));
                dataModel = DataModel<PokemonModel>.FromByteArray(d.Data);
                _pokemonModelCache.Add(pokemonId, dataModel);
                return CreatePokemon(dataModel, level);
            }
            else
            {
                return CreatePokemon(dataModel, level);
            }
        }
        
        public Pokemon GetPokemon(PokemonSaveModel saveModel)
        {
            PokemonModel dataModel;

            if (!_pokemonModelCache.TryGetValue(saveModel.Id, out dataModel))
            {
                var d = _gameMode.FileLoader.GetFile(_gameMode.GetPokemonFilePath(saveModel.Id));
                if (!_pokemonModelCache.ContainsKey(saveModel.Id))
                {
                    dataModel = DataModel<PokemonModel>.FromByteArray(d.Data);
                    _pokemonModelCache.Add(saveModel.Id, dataModel);
                }
                else
                {
                    dataModel = _pokemonModelCache[saveModel.Id];
                }
                return new Pokemon(_gameMode, dataModel, saveModel);
            }
            else
            {
                return new Pokemon(_gameMode, dataModel, saveModel);
            }
        }
        
        private Pokemon CreatePokemon(PokemonModel dataModel, int level)
        {
            var saveModel = new PokemonSaveModel();
            PopulateSaveModel(dataModel, saveModel);
            var pokemon = new Pokemon(_gameMode, dataModel, saveModel);

            pokemon.LearnStartupMoves();
            if (level > 1)
                pokemon.LevelUp(true, level);

            return pokemon;
        }
        
        /// <summary>
        /// Generates a personality value for a Pokémon from 10 random numbers and letters.
        /// </summary>
        private string GeneratePersonalityValue()
        {
            if (_charCodes == null)
            {
                // ASCII code table (https://en.wikipedia.org/wiki/ASCII#ASCII_printable_code_chart)
                // initialize char code array with letters and numbers:
                var charCodes = new List<int>();
                charCodes.AddRange(Enumerable.Range(48, 10)); // 0-9
                charCodes.AddRange(Enumerable.Range(65, 26)); // A-Z
                charCodes.AddRange(Enumerable.Range(97, 26)); // a-z

                _charCodes = charCodes.ToArray();
            }

            var personalityValue = string.Empty;

            while (personalityValue.Length < PersonalityValueLength)
                personalityValue += ((char)_charCodes[GlobalRandomProvider.Instance.Rnd.Next(0, _charCodes.Length)]).ToString();

            return personalityValue;
        }

        private void PopulateSaveModel(PokemonModel dataModel, PokemonSaveModel saveModel)
        {
            // when generating a new Pokémon, the save model is empty, as it is a blank slate.
            // some values are generated by default or just set to their base values, so we do that here.

            saveModel.Id = dataModel.Id;

            // setting personality value:
            saveModel.PersonalityValue = GeneratePersonalityValue();

            // setting gender:
            if (dataModel.IsGenderless)
            {
                saveModel.Gender = PokemonGender.Genderless;
            }
            else
            {
                var r = GlobalRandomProvider.Instance.Rnd.NextDouble();
                saveModel.Gender = r <= (dataModel.IsMale / 100) ? PokemonGender.Male : PokemonGender.Female;
            }

            // presetting catch info with empty information:
            saveModel.CatchInfo = new PokemonCatchInfo()
            {
                BallItemId = "",
                Location = "",
                Method = "",
                TrainerName = "",
                OT = ""
            };

            // no egg, 0 experience:
            saveModel.EggSteps = 0;
            saveModel.Experience = 0;

            // set all effort values to 0:
            saveModel.EVs = new PokemonStatSetModel()
            {
                Atk = 0,
                Def = 0,
                SpAtk = 0,
                SpDef = 0,
                HP = 0,
                Speed = 0
            };
            // randomize IVs:
            saveModel.IVs = new PokemonStatSetModel()
            {
                Atk = GlobalRandomProvider.Instance.Rnd.Next(0, 32),
                Def = GlobalRandomProvider.Instance.Rnd.Next(0, 32),
                SpAtk = GlobalRandomProvider.Instance.Rnd.Next(0, 32),
                SpDef = GlobalRandomProvider.Instance.Rnd.Next(0, 32),
                HP = GlobalRandomProvider.Instance.Rnd.Next(0, 32),
                Speed = GlobalRandomProvider.Instance.Rnd.Next(0, 32)
            };

            // set to random nature:
            var natureModels = _gameMode.GetNatures();
            saveModel.NatureId = natureModels[_random.Next(natureModels.Length)].Id;

            // set to base friendship:
            saveModel.Friendship = dataModel.BaseFriendship;

            // chance of 1/4096 to be shiny:
            saveModel.IsShiny = (GlobalRandomProvider.Instance.Rnd.Next(0, 4096) == 0);

            saveModel.Nickname = "";

            saveModel.Status = PokemonStatus.None;

            saveModel.AdditionalData = "";

            saveModel.Moves = new PokemonMoveModel[0];
        }
    }
}
