using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.Pokemon;

namespace Pokemon3D.GameModes.Pokemon
{
    class PokemonStatCalculator
    {
        public static int CalculateHP(int baseHP, int IVHP, int EVHP, int level)
        {
            // HP = 
            // (floor((2 * Base + IV + floor(EV / 4)) * Level) + Level + 10)

            return (int)(Math.Floor(((2 * baseHP + IVHP + Math.Floor((double)EVHP / 4)) * level) / 100) + level + 10);
        }

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

            return (int)((Math.Floor((Math.Floor(2 * baseStat + IV + Math.Floor((double)EV / 4)) * pokemon.Level) / 100) + 5) * nature);
        }
    }
}
