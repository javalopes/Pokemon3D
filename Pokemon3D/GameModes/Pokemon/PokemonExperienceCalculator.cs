using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.DataModel.Json.GameMode.Pokemon;

namespace Pokemon3D.GameModes.Pokemon
{
    /// <summary>
    /// A class that contains methods to work with Pokémon experience formulas.
    /// </summary>
    static class PokemonExperienceCalculator
    {
        #region Public methods

        public static int ExperienceNeededForLevel(ExperienceType experienceType, int level)
        {
            switch (experienceType)
            {
                case ExperienceType.Erratic:
                    return (int)Math.Round(ExperienceNeededForLevelErratic(level));
                case ExperienceType.Fast:
                    return (int)Math.Round(ExperienceNeededForLevelFast(level));
                case ExperienceType.MediumFast:
                    return (int)Math.Round(ExperienceNeededForLevelMediumFast(level));
                case ExperienceType.MediumSlow:
                    return (int)Math.Round(ExperienceNeededForLevelMediumSlow(level));
                case ExperienceType.Slow:
                    return (int)Math.Round(ExperienceNeededForLevelSlow(level));
                case ExperienceType.Fluctuating:
                    return (int)Math.Round(ExperienceNeededForLevelFluctuating(level));
                default:
                    return (int)Math.Round(ExperienceNeededForLevelMediumFast(level));
            }
        }

        /// <summary>
        /// Returns the equivalent level for an experience value.
        /// </summary>
        public static int LevelForExperienceValue(ExperienceType experienceType, int experience)
        {
            // returns level 1 if no experience (or negative value):
            if (experience <= 0)
                return 1;

            int level = 1;
            while (ExperienceNeededForLevel(experienceType, level) <= experience)
                level++;

            return level;
        }

        #endregion

        // for simpler writing, "level" is refered to as "n":

        private static double ExperienceNeededForLevelErratic(double n)
        {
            // EXP = 
            // n <= 50:         ((pow(n,3) * (100 - n)) / 50)
            // 50 <= n <= 68:   ((pow(n,3) * (150 - n)) / 100)
            // 68 <= n <= 98:   ((pow(n,3) * floor((1911 - (10 * n)) / 3)) / 500)
            // 98 <= n <= 100:  ((pow(n,3) * (160 - n)) / 100)

            if (n <= 50)
                return ((Math.Pow(n, 3) * (100 - n)) / 50);
            else if (50 <= n && n <= 68)
                return ((Math.Pow(n, 3) * (150 - n)) / 100);
            else if (68 <= n && n <= 98)
                return ((Math.Pow(n, 3) * Math.Floor((1911 - (10 * n)) / 3)) / 500);
            else //if (98 <= n && n <= 100)
                return ((Math.Pow(n, 3) * (160 - n)) / 100);
        }

        private static double ExperienceNeededForLevelFast(double n)
        {
            // EXP = 
            // ((4 * pow(n,3)) / 5)

            return ((4 * Math.Pow(n, 3)) / 5);
        }

        private static double ExperienceNeededForLevelMediumFast(double n)
        {
            // EXP =
            // (pow(n,3))

            return (Math.Pow(n, 3));
        }

        private static double ExperienceNeededForLevelMediumSlow(double n)
        {
            // EXP = 
            // (((6 / 5) * pow(n,3)) - (15 * pow(n,2)) + (100 * n) - 140)

            return (((6 / 5) * Math.Pow(n, 3)) - (15 * Math.Pow(n, 2)) + (100 * n) - 140);
        }

        private static double ExperienceNeededForLevelSlow(double n)
        {
            // EXP = 
            // ((5 * pow(n,3)) / 4)

            return ((5 * Math.Pow(n, 3)) / 4);
        }

        private  static double ExperienceNeededForLevelFluctuating(double n)
        {
            // EXP = 
            // n <= 15: (pow(n,3) * ((floor((n + 1) / 3) + 24) / 50))
            // 15 <= n <= 36: (pow(n,3) * ((n + 14) / 50))
            // 36 <= n <= 100: (pow(n,3) * ((floor(n / 2) + 32) / 50))

            if (n <= 15)
                return (Math.Pow(n, 3) * ((Math.Floor((n + 1) / 3) + 24) / 50));
            else if (15 <= n && n <= 36)
                return (Math.Pow(n, 3) * ((n + 14) / 50));
            else //if (36 <= n && n <= 100)
                return (Math.Pow(n, 3) * ((Math.Floor(n / 2) + 32) / 50));
        }
    }
}
