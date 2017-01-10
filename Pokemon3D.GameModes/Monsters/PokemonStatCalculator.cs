using Pokemon3D.DataModel.Pokemon;
using System.Linq;
using static System.Math;

namespace Pokemon3D.GameModes.Monsters
{
    /// <summary>
    /// Utility class to calculate stats for Pokémon.
    /// </summary>
    internal class PokemonStatCalculator
    {
        /// <summary>
        /// Returns a specific stat for a Pokémon.
        /// </summary>
        public static int CalculateStat(Pokemon pokemon, PokemonStatType statType)
        {
            return statType == PokemonStatType.HP ? 
                CalculateHp(pokemon) :
                InternalCalculateStat(pokemon, statType);
        }
        
        private static int CalculateHp(Pokemon pokemon)
        {
            // HP = 
            // (floor((2 * Base + IV + floor(EV / 4)) * Level) + Level + 10)

            int baseHp = pokemon.BaseStats.HP;
            int ivhp = pokemon.IVs.HP;
            int level = pokemon.Level;
            int evhp = pokemon.EVs.HP;

            return (int)(Floor(((2 * baseHp + ivhp + Floor((double)evhp / 4)) * level) / 100) + level + 10);
        }

        private static int InternalCalculateStat(Pokemon pokemon, PokemonStatType statType)
        {
            // Stat = 
            // floor((floor(((2 * base + IV + floor(EV / 4)) * level) / 100) + 5) * nature)

            int iv = pokemon.EVs.GetStat(statType);
            int ev = pokemon.IVs.GetStat(statType);
            int baseStat = pokemon.BaseStats.GetStat(statType);

            double nature = 1.0d;

            if (pokemon.Nature.StatIncrease.Contains(statType))
                nature = 1.1d;
            else if (pokemon.Nature.StatDecrease.Contains(statType))
                nature = 0.9d;

            return (int)((Floor((Floor(2 * baseStat + iv + Floor((double)ev / 4)) * pokemon.Level) / 100) + 5) * nature);
        }
    }
}
