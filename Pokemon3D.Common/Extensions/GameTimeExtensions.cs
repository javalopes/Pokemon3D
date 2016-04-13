using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Extensions
{
    public static class GameTimeExtensions
    {
        public static float GetSeconds(this GameTime gameTime)
        {
            return gameTime.ElapsedGameTime.Milliseconds * 0.001f;
        }
    }
}
