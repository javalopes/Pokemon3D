using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.InputSystem
{
    public class GamePadInputAction : InputAction
    {
        private readonly GamePadHandler _actionProvider;
        private readonly Buttons _button;

        public GamePadInputAction(GamePadHandler actionProvider, string name, Buttons button)
        {
            Name = name;
            _actionProvider = actionProvider;
            _button = button;
        }

        public override bool IsPressed()
        {
            return _actionProvider.IsButtonDown(_button);
        }

        public override bool IsPressedOnce()
        {
            return _actionProvider.IsButtonDownOnce(_button);
        }
    }
}