using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.Common.Input
{
    internal class KeyboardAxisAction : AxisAction
    {
        private readonly KeyboardHandler _actionProvider;
        private readonly Keys _left;
        private readonly Keys _right;
        private readonly Keys _up;
        private readonly Keys _down;

        public KeyboardAxisAction(KeyboardHandler actionProvider, string name, Keys left, Keys right, Keys up, Keys down)
        {
            _actionProvider = actionProvider;
            _left = left;
            _right = right;
            _up = up;
            _down = down;
            Name = name;
        }

        public override Vector2 GetAxis()
        {
            var vector = Vector2.Zero;
            if (_actionProvider.IsKeyDown(_left)) vector.X = -1;
            else if (_actionProvider.IsKeyDown(_right)) vector.X = 1;

            if (_actionProvider.IsKeyDown(_up)) vector.Y = 1;
            else if (_actionProvider.IsKeyDown(_down)) vector.Y = -1;

            return vector;
        }
    }
}