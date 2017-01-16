using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.InputSystem
{
    public class MouseHandler
    {
        private MouseState _lastMouseState;
        private MouseState _currentMouseState;

        public MouseHandler()
        {
            _currentMouseState = Mouse.GetState();
        }

        public void Update()
        {
            _lastMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        public bool IsLeftButtonDown()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsRightButtonDown()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed;
        }

        public bool IsLeftButtonDownOnce()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released;
        }

        public bool IsRightButtonDownOnce()
        {
            return _currentMouseState.RightButton == ButtonState.Pressed && _lastMouseState.RightButton == ButtonState.Released;
        }

        public int DeltaX => _currentMouseState.X - _lastMouseState.X;

        public int DeltaY => _currentMouseState.Y - _lastMouseState.Y;

        public int X => _currentMouseState.X;

        public int Y => _currentMouseState.Y;
    }
}