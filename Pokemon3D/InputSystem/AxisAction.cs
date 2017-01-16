using Microsoft.Xna.Framework;

namespace Pokemon3D.InputSystem
{
    public abstract class AxisAction
    {
        public string Name { get; protected set; }

        public abstract Vector2 GetAxis();
    }
}