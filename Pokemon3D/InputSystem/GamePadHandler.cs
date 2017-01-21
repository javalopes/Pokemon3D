using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.InputSystem
{
    public class GamePadHandler
    {
        private GamePadState _lastState;
        private GamePadState _currentState;

        public GamePadHandler()
        {
            _currentState = GamePad.GetState(PlayerIndex.One);
        }

        public InputAction DefineAction(string name, Buttons button)
        {
            return new GamePadInputAction(this, name, button);
        }

        public AxisAction DefineAxis(string name, GamePadAxis axis)
        {
            return new GamePadAxisAction(this, name, axis);
        }

        public bool IsButtonDownOnce(Buttons button)
        {
            return _currentState.IsButtonDown(button) && _lastState.IsButtonUp(button);
        }

        public bool IsButtonDown(Buttons button)
        {
            return _currentState.IsButtonDown(button);
        }

        public bool IsButtonUp(Buttons button)
        {
            return _currentState.IsButtonUp(button);
        }

        public bool IsConnected()
        {
            return GamePad.GetState(PlayerIndex.One).IsConnected;
        }

        public Vector2 GetAxis(GamePadAxis axis)
        {
            switch (axis)
            {
                case GamePadAxis.ThumbStickLeft:
                    return _currentState.ThumbSticks.Left;
                case GamePadAxis.ThumbStickRight:
                    return _currentState.ThumbSticks.Right;
                case GamePadAxis.DigitalPad:
                    return GetDigitalPadAxis();
                default:
                    throw new ArgumentOutOfRangeException(nameof(axis), axis, null);
            }
        }

        private Vector2 GetDigitalPadAxis()
        {
            var vector = Vector2.Zero;

            if (_currentState.DPad.Left == ButtonState.Pressed)
            {
                vector.X = -1;
            }
            else if (_currentState.DPad.Right == ButtonState.Pressed)
            {
                vector.X = 1;
            }

            if (_currentState.DPad.Up == ButtonState.Pressed)
            {
                vector.Y = -1;
            }
            else if (_currentState.DPad.Down == ButtonState.Pressed)
            {
                vector.Y = 1;
            }

            return vector;
        }

        public void Update(GameTime time)
        {
            _lastState = _currentState;
            _currentState = GamePad.GetState(PlayerIndex.One);
        }
    }
}