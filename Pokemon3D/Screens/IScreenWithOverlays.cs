using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.UI;
using Pokemon3D.UI;

namespace Pokemon3D.Screens
{
    internal abstract class IScreenWithOverlays : IScreen
    {
        private readonly ScreenOverlay _screenOverlay = new ScreenOverlay();

        public virtual void OnEarlyDraw(GameTime gameTime)
        {
            
        }

        public virtual void OnClosing()
        {
            
        }

        public abstract void OnOpening(object enterInformation);

        public virtual void OnLateDraw(GameTime gameTime)
        {
            _screenOverlay.Draw();
        }

        public virtual void OnUpdate(GameTime gameTime)
        {
            _screenOverlay.Update(gameTime);
        }
        
        public UiOverlay AddOverlay(UiOverlay overlay)
        {
            _screenOverlay.AddOverlay(overlay);
            return overlay;
        }

        public void RemoveOverlay(UiOverlay overlay)
        {
            _screenOverlay.RemoveOverlay(overlay);
        }
    }
}