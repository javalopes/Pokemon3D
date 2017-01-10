using System;

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
