using Pokemon3D.GameModes.Monsters;

namespace Pokemon3D.GameModes.Battle
{
    /// <summary>
    /// Controls additional properties assigned to a Pokémon during a battle.
    /// </summary>
    class BattlePokemon
    {
        public Pokemon Pokemon { get; }

        public BattleFieldPosition Position { get; set; }

        public BattlePokemon(Pokemon pokemon, BattleFieldPosition position)
        {
            Pokemon = pokemon;
            Position = position;
        }
    }
}
