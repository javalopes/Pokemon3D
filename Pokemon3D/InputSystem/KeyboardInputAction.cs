using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.InputSystem
{
    internal class KeyboardInputAction : InputAction
    {
        private readonly KeyboardHandler _actionProvider;
        private readonly Keys _key;

        public KeyboardInputAction(KeyboardHandler actionProvider, string name, Keys key)
        {
            _actionProvider = actionProvider;
            _key = key;
            Name = name;
        }

        public override bool IsPressed()
        {
            return _actionProvider.IsKeyDown(_key);
        }

        public override bool IsPressedOnce()
        {
            return _actionProvider.IsKeyDownOnce(_key);
        }
    }
}