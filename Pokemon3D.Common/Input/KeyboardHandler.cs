using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.Common.Input
{
    public class KeyboardHandler
    {
        private KeyboardState _lastState;
        private KeyboardState _currentState;
        private Keys[] _pressedKeys;
 
        public KeyboardHandler()
        {
            _currentState = Keyboard.GetState();
        }

        public InputAction DefineAction(string name, Keys key)
        {
            return new KeyboardInputAction(this, name, key);
        }

        public AxisAction DefineAxis(string name, Keys left, Keys right, Keys up, Keys down)
        {
            return new KeyboardAxisAction(this, name, left, right, up, down);
        }

        public bool IsKeyDownOnce(Keys key)
        {
            return _currentState.IsKeyDown(key) && _lastState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        public Keys[] GetPressedKeys()
        {
            return _pressedKeys;
        }

        public void Update(GameTime time)
        {
            _lastState = _currentState;
            _currentState = Keyboard.GetState();
            _pressedKeys = _currentState.GetPressedKeys();
        }
    }
}