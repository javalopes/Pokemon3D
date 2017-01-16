using Microsoft.Xna.Framework;

namespace Pokemon3D.Common
{
    public interface GameContext
    {
        TService GetService<TService>() where TService : class;

        Rectangle ScreenBounds { get; }
    }
}
