using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.GameModes.Battle
{
    /// <summary>
    /// Main class to control the battle happenings.
    /// </summary>
    internal class BattleController
    {
        public Random Generator { get; }

        public BattleController(Random generator)
        {
            Generator = generator;
        }
    }
}
