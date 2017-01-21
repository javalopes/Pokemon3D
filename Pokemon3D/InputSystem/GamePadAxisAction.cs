using Microsoft.Xna.Framework;

namespace Pokemon3D.InputSystem
{
    public class GamePadAxisAction : AxisAction
    {
        private readonly GamePadHandler _actionProvider;
        private readonly GamePadAxis _axis;

        public GamePadAxisAction(GamePadHandler actionProvider, string name, GamePadAxis axis)
        {
            Name = name;
            _actionProvider = actionProvider;
            _axis = axis;
        }

        public override Vector2 GetAxis()
        {
            return _actionProvider.GetAxis(_axis);
        }
    }
}