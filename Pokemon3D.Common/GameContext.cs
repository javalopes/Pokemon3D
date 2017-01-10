using Microsoft.Xna.Framework;
using System;

namespace Pokemon3D.Common
{
    public interface GameContext
    {
        void EnsureExecutedInMainThread(Action action);

        TService GetService<TService>() where TService : class;

        Rectangle ScreenBounds { get; }
    }
}
