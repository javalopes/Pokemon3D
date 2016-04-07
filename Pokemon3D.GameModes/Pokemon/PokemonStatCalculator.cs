using Pokemon3D.DataModel.Pokemon;
using System.Linq;
using static System.Math;

namespace Pokemon3D.Entities.Pokemon
{
    /// <summary>
    /// Utility class to calculate stats for Pokémon.
    /// </summary>
    class PokemonStatCalculator
    {
        /// <summary>
        /// Returns the max HP stat for a Pokémon.
        /// </summary>
        public static int CalculateHP(Pokemon pokemon)
        {
            // HP = 
            // (floor((2 * Base + IV + floor(EV / 4)) * Level) + Level + 10)

            int baseHP = pokemon.BaseStats.HP;
            int IVHP = pokemon.IVs.HP;
            int level = pokemon.Level;
            int EVHP = pokemon.EVs.HP;

            return (int)(Floor(((2 * baseHP + IVHP + Floor((double)EVHP / 4)) * level) / 100) + level + 10);
        }

        /// <summary>
        /// Returns a specific stat for a Pokémon.
        /// </summary>
        public static int CalculateStat(Pokemon pokemon, PokemonStatType statType)
        {
            // Stat = 
            // floor((floor(((2 * base + IV + floor(EV / 4)) * level) / 100) + 5) * nature)

            int IV = pokemon.EVs.GetStat(statType);
            int EV = pokemon.IVs.GetStat(statType);
            int baseStat = pokemon.BaseStats.GetStat(statType);

            double nature = 1.0d;

            if (pokemon.Nature.StatIncrease.Contains(statType))
                nature = 1.1d;
            else if (pokemon.Nature.StatDecrease.Contains(statType))
                nature = 0.9d;

            return (int)((Floor((Floor(2 * baseStat + IV + Floor((double)EV / 4)) * pokemon.Level) / 100) + 5) * nature);
        }
    }
}
