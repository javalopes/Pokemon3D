using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.Common.Input
{
    public class GamePadActionProvider
    {
        private GamePadState _lastState;
        private GamePadState _currentState;

        public GamePadActionProvider()
        {
            _currentState = GamePad.GetState(PlayerIndex.One);
        }

        internal bool IsButtonDownOnce(Buttons button)
        {
            return _currentState.IsButtonDown(button) && _lastState.IsButtonUp(button);
        }

        internal bool IsButtonDown(Buttons button)
        {
            return _currentState.IsButtonDown(button);
        }

        internal bool IsButtonUp(Buttons button)
        {
            return _currentState.IsButtonUp(button);
        }

        public bool IsConnected()
        {
            return GamePad.GetState(PlayerIndex.One).IsConnected;
        }

        public void Update(GameTime time)
        {
            _lastState = _currentState;
            _currentState = GamePad.GetState(PlayerIndex.One);
        }
    }
}