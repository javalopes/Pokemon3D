using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.DataModel.GameMode.Battle;
using static System.Math;

namespace Pokemon3D.GameModes.Battle
{
    static class BattleTarget
    {
        public static IEnumerable<BattlePokemon> GetTargetablePokemon(IEnumerable<BattlePokemon> allPokemon, BattlePokemon user, TargetType target)
        {
            if (target == TargetType.SingleTarget || 
                target == TargetType.MultiTarget)
                throw new InvalidOperationException("Collection target types cannot be used as a target description.");

            var userPos = user.Position;
            switch (target)
            {
                case TargetType.Self:
                    return allPokemon.Where(p => p.Position == userPos);

                case TargetType.OneAdjacentTarget:
                case TargetType.AllAdjacentTargets:
                    return allPokemon.Where(p => p.Position != userPos && Abs(p.Position.X - userPos.X) == 1);
                case TargetType.OneAdjacentFoe:
                case TargetType.AllAdjacentFoes:
                    return allPokemon.Where(p => p.Position != userPos && Abs(p.Position.X - userPos.X) == 1 && p.Position.Y != userPos.Y);
                case TargetType.OneAdjacentAlly:
                case TargetType.AllAdjacentAllies:
                    return allPokemon.Where(p => p.Position != userPos && Abs(p.Position.X - userPos.X) == 1 && p.Position.Y == userPos.Y);
                    
                case TargetType.OneTarget:
                case TargetType.AllTargets:
                    return allPokemon.Where(p => p.Position != userPos);
                case TargetType.OneFoe:
                case TargetType.AllFoes:
                    return allPokemon.Where(p => p.Position.Y != userPos.Y);
                case TargetType.OneAlly:
                case TargetType.AllAllies:
                    return allPokemon.Where(p => p.Position != userPos && p.Position.Y == userPos.Y);
                    
                case TargetType.AllOwn:
                    return allPokemon.Where(p => p.Position.Y == userPos.Y);

                case TargetType.All:
                default:
                    return allPokemon;
            }
        }

        public static bool IsMultiTarget(TargetType target) => TargetType.MultiTarget.HasFlag(target);
    }
}
