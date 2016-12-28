using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Input
{
    public abstract class AxisAction
    {
        public string Name { get; protected set; }

        public abstract Vector2 GetAxis();
    }
}