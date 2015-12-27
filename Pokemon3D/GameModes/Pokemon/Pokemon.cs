using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Pokemon;
using Pokemon3D.DataModel.Json.Savegame.Pokemon;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// A Pokémon instance that holds the definition and save data of a Pokémon instance.
    /// </summary>
    class Pokemon
    {
        private PokemonModel _dataModel;
        private PokemonSaveModel _saveModel;

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

        public Pokemon(PokemonModel dataModel)
        {
            _dataModel = dataModel;
        }

        public Pokemon(PokemonModel dataModel, PokemonSaveModel saveModel)
        {
            _dataModel = dataModel;
            _saveModel = saveModel;
        }

    }
}
