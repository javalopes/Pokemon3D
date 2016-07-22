using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using System;

namespace Pokemon3D.Common
{
    public interface GameContext
    {
        ContentManager Content { get; }
        Rectangle ScreenBounds { get; }
        string VersionInformation { get; }
        void EnsureExecutedInMainThread(Action action);

        TService GetService<TService>() where TService : class;
    }
}
