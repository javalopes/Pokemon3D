namespace Pokemon3D.GameModes.Battle
{
    /// <summary>
    /// Describes the position of a Pokémon on the battlefield.
    /// </summary>
    struct BattleFieldPosition
    {
        /// <summary>
        /// The position of the Pokémon on the side (left/right).
        /// </summary>
        public readonly int X;
        /// <summary>
        /// The side of the Pokémon (own Pokémon/opponent's Pokémon).
        /// </summary>
        public readonly int Y;

        public BattleFieldPosition(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Equality

        public override bool Equals(object obj)
        {
            return obj is BattleFieldPosition ? (BattleFieldPosition)obj == this : false;
        }

        public static bool operator ==(BattleFieldPosition pos1, BattleFieldPosition pos2)
        {
            return pos1.X == pos2.X && pos1.Y == pos2.Y;
        }

        public static bool operator !=(BattleFieldPosition pos1, BattleFieldPosition pos2)
        {
            return !(pos1 == pos2);
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        #endregion
    }
}
