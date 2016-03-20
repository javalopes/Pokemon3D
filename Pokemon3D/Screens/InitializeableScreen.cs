using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;

namespace Pokemon3D.Screens
{
    abstract class InitializeableScreen : GameObject, Screen
    {
        private bool _isInitialized;

        protected abstract void OnInitialize(object enterInformation);
        public abstract void OnDraw(GameTime gameTime);
        public abstract void OnUpdate(float elapsedTime);
        public abstract void OnClosing();

        public virtual void OnOpening(object enterInformation)
        {
            if (!_isInitialized) OnInitialize(enterInformation);
            _isInitialized = true;
        }
    }
}
