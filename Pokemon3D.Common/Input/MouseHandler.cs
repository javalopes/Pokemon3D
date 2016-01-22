using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.Common.Input
{
    public class MouseHandler
    {
        private MouseState _lastState;
        private MouseState _currentState;

        public MouseHandler()
        {
            _currentState = Mouse.GetState();
        }

        private ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Right:
                    return state.RightButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
            }
            return ButtonState.Released;
        }

        public bool IsButtonDownOnce(MouseButton button)
        {
            return GetButtonState(_currentState, button) == ButtonState.Pressed &&
                GetButtonState(_lastState, button) == ButtonState.Released;
        }

        public bool IsButtonDown(MouseButton button)
        {
            return GetButtonState(_currentState, button) == ButtonState.Pressed;
        }

        public bool IsButtonUp(MouseButton button)
        {
            return GetButtonState(_currentState, button) == ButtonState.Released;
        }

        public Point Position
        {
            get { return _currentState.Position; }
        }

        public int GetScrollWheelDifference()
        {
            return _currentState.ScrollWheelValue - _lastState.ScrollWheelValue;
        }

        public bool HasMoved
        {
            get
            {
                return _currentState.Position != _lastState.Position;
            }
        }

        public void Update()
        {
            _lastState = _currentState;
            _currentState = Mouse.GetState();
        }
    }
}
