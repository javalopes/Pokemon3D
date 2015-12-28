using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Pokemon;
using Pokemon3D.DataModel.Json.Savegame.Pokemon;
using Pokemon3D.DataModel.Json.Pokemon;
using Pokemon3D.DataModel.Json.GameMode.Definitions;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// A Pokémon instance that holds the definition and save data of a Pokémon instance.
    /// </summary>
    class Pokemon
    {
        private const string DEFAULT_FORM_VALUE = "Default";

        private GameMode _gameMode;

        private PokemonModel _dataModel;
        private PokemonSaveModel _saveModel;

        private string _activeForm;

        /// <summary>
        /// The effort values of this Pokémon.
        /// </summary>
        /// <remarks>http://bulbapedia.bulbagarden.net/wiki/Effort_values</remarks>
        public PokemonStatSetModel EVs
        {
            get { return _saveModel.EVs; }
        }

        /// <summary>
        /// The individual values of this Pokémon.
        /// </summary>
        /// <remarks>http://bulbapedia.bulbagarden.net/wiki/Individual_values</remarks>
        public PokemonStatSetModel IVs
        {
            get { return _saveModel.IVs; }
        }

        /// <summary>
        /// The base stats of this Pokémon.
        /// </summary>
        /// <remarks>http://bulbapedia.bulbagarden.net/wiki/Statistic</remarks>
        public PokemonStatSetModel BaseStats
        {
            get { return ActiveFormModel.BaseStats; }
        }

        /// <summary>
        /// The current health points of this Pokémon.
        /// </summary>
        public int HP
        {
            get { return _saveModel.HP; }
            set { _saveModel.HP = value; }
        }

        public int Experience
        {
            get { return _saveModel.Experience; }
            set { _saveModel.Experience = value; }
        }

        public int Level
        {
            get
            {
                return PokemonExperienceCalculator.LevelForExperienceValue(_dataModel.ExperienceType, Experience);
            }
            set
            {
                // when the level is set from outside, the experience is set to exactly reach that level.
                int experienceNeeded = PokemonExperienceCalculator.ExperienceNeededForLevel(_dataModel.ExperienceType, value);
                Experience = experienceNeeded;
            }
        }

        public NatureModel Nature
        {
            get { return _gameMode.NatureManager.GetNature(_saveModel.NatureId); }
        }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_saveModel.Nickname))
                    return _saveModel.Nickname;
                else
                    return _dataModel.Name;
            }
        }

        private PokemonFormModel ActiveFormModel
        {
            get { return _dataModel.Forms.Single(x => x.Id == _activeForm); }
        }
        
        public Pokemon(GameMode gameMode, PokemonModel dataModel, PokemonSaveModel saveModel)
        {
            _gameMode = gameMode;

            _dataModel = dataModel;
            _saveModel = saveModel;

            FormChangeTriggerHandle();
        }

        private void FormChangeTriggerHandle()
        {
            // determine the current form of the Pokémon here.
            // if no special form applies, set to "Default".

            _activeForm = DEFAULT_FORM_VALUE;
        }

    }
}
