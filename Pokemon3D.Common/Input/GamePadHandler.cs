using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Common.Input
{
    public class GamePadHandler
    {
        private GamePadState _lastState;
        private GamePadState _currentState;

        public GamePadHandler()
        {
            _currentState = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsButtonDownOnce(Buttons button)
        {
            return _currentState.IsButtonDown(button) && _currentState.IsButtonUp(button);
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

        public void Update()
        {
            _lastState = _currentState;
            _currentState = GamePad.GetState(PlayerIndex.One);
        }
    }
}
