using Pokemon3D.DataModel.GameMode.Pokemon;
using Pokemon3D.DataModel.Pokemon;
using Pokemon3D.DataModel.Savegame.Pokemon;
using Pokemon3D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel.GameMode.Definitions;

namespace Pokemon3D.GameModes.Pokemon
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

        private const int PERSONALITY_VALUE_LENGTH = 10;

        public PokemonFactory(GameMode gameMode)
        {
            _gameMode = gameMode;
        }

        private string CreateKey(string pokemonId)
        {
            return _gameMode.GetPokemonFilePath(pokemonId);
        }

        //public object GetRequestOrPokemon(PokemonSaveModel saveModel)
        //{
        //    string key = CreateKey(saveModel.Id);
        //    if (_pokemonCache.ContainsKey(key))
        //    {
        //        var dataModel = _pokemonCache[key];
        //        return new Pokemon(_gameMode, dataModel, saveModel);
        //    }
        //    else
        //    {
        //        return CreateDataRequest(saveModel);
        //    }
        //}

        ///// <summary>
        ///// Creates a data request from an already existing save model.
        ///// </summary>
        //public PokemonDataRequest CreateDataRequest(PokemonSaveModel saveModel)
        //{
        //    var request = new PokemonDataRequest(_gameMode, saveModel);
        //    request.Finished += FinishedRequest;
        //    return request;
        //}

        //public object GetRequestOrPokemon(string pokemonId, int level)
        //{
        //    string key = CreateKey(pokemonId);
        //    if (_pokemonCache.ContainsKey(key))
        //    {
        //        var dataModel = _pokemonCache[key];
        //        var saveModel = new PokemonSaveModel();
        //        PopulateSaveModel(dataModel, saveModel, level);
        //        return new Pokemon(_gameMode, dataModel, saveModel);
        //    }
        //    else
        //    {
        //        return CreateDataRequest(pokemonId, level);
        //    }
        //}

        ///// <summary>
        ///// Creates a data request from a path to a data model and the starting level of the Pokémon.
        ///// </summary>
        //public PokemonDataRequest CreateDataRequest(string pokemonId, int level)
        //{
        //    var request = new PokemonDataRequest(_gameMode, CreateKey(pokemonId), level);
        //    request.Finished += FinishedRequest;
        //    return request;
        //}

        //private void FinishedRequest(object sender, EventArgs e)
        //{
        //    var request = (PokemonDataRequest)sender;

        //    if (_pokemonCache.ContainsKey(request.DataPath))
        //        _pokemonCache[request.DataPath] = request.ResultModel;
        //    else
        //        _pokemonCache.Add(request.DataPath, request.ResultModel);

        //    if (request.SaveModel == null)
        //    {
        //        request.SaveModel = new PokemonSaveModel();
        //        PopulateSaveModel(request.ResultModel, request.SaveModel, request.StartLevel);
        //    }

        //    var pokemon = new Pokemon(_gameMode, request.ResultModel, request.SaveModel);

        //    pokemon.LearnStartupMoves();
        //    if (request.StartLevel > 1)
        //    {
        //        pokemon.LevelUp(true, request.StartLevel - 1);
        //    }

        //    request.ResultPokemon = pokemon;
        //}
        
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

            while (personalityValue.Length < PERSONALITY_VALUE_LENGTH)
                personalityValue += ((char)_charCodes[GlobalRandomProvider.Instance.Rnd.Next(0, _charCodes.Length)]).ToString();

            return personalityValue;
        }

        private void PopulateSaveModel(PokemonModel dataModel, PokemonSaveModel saveModel, int level)
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
                if (r <= (dataModel.IsMale / 100))
                {
                    saveModel.Gender = PokemonGender.Male;
                }
                else
                {
                    saveModel.Gender = PokemonGender.Female;
                }
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
