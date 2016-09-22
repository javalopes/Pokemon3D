using System;

namespace Pokemon3D.DataModel.GameMode.Battle
{
    /// <summary>
    /// Which Pokémon a move can target.
    /// </summary>
    [Flags]
    public enum TargetType
    {
        /// <summary>
        /// One adjacent target, excluding itself.
        /// </summary>
        OneAdjacentTarget = 0,
        /// <summary>
        /// One adjacent foe.
        /// </summary>
        OneAdjacentFoe = 1 << 0,
        /// <summary>
        /// One adjacent ally, excluding itself.
        /// </summary>
        OneAdjacentAlly = 1 << 1,

        /// <summary>
        /// One target, excluding itself.
        /// </summary>
        OneTarget = 1 << 2,
        /// <summary>
        /// One Foe.
        /// </summary>
        OneFoe = 1 << 3,
        /// <summary>
        /// One ally, excluding itself.
        /// </summary>
        OneAlly = 1 << 4,

        /// <summary>
        /// Only itself.
        /// </summary>
        Self = 1 << 5,

        /// <summary>
        /// All adjacent targets, exluding itself.
        /// </summary>
        AllAdjacentTargets = 1 << 6,
        /// <summary>
        /// All adjacent foes.
        /// </summary>
        AllAdjacentFoes = 1 << 7,
        /// <summary>
        /// All adjacent allies, excluding itself.
        /// </summary>
        AllAdjacentAllies = 1 << 8,

        /// <summary>
        /// All Targets, excluding itself.
        /// </summary>
        AllTargets = 1 << 9,
        /// <summary>
        /// All Foes.
        /// </summary>
        AllFoes = 1 << 10,
        /// <summary>
        /// All allies, excluding itself.
        /// </summary>
        AllAllies = 1 << 11,
        /// <summary>
        /// All own Pokémon (allies + itself).
        /// </summary>
        AllOwn = 1 << 12,

        /// <summary>
        /// All Pokémon, including itself.
        /// </summary>
        All = 1 << 13,
        
        // collection entries:

        /// <summary>
        /// Contains all target types that require a single target.
        /// </summary>
        SingleTarget = OneAdjacentTarget | OneAdjacentFoe | OneAdjacentAlly | OneTarget | OneFoe | OneAlly | Self,
        /// <summary>
        /// Contains all target types that require at least one target.
        /// </summary>
        MultiTarget = AllAdjacentTargets | AllAdjacentFoes | AllAdjacentAllies | AllTargets | AllFoes | AllAllies | All
    }
}
