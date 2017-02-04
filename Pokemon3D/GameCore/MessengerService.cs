using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.GameCore
{
    internal class MessengerService
    {
        private readonly UiOverlay _overlay;
        private MessageData _activeMessage;
        private readonly SpriteBatch _spriteBatch;

        public MessengerService()
        {
            _overlay = new UiOverlay();
            _overlay.Hidden += OverlayOnHidden;
            _spriteBatch = GameInstance.GetService<SpriteBatch>();
        }

        private void OverlayOnHidden()
        {
            _activeMessage = null;
        }

        public void ShowMessage(MessageData messageData)
        {
            _activeMessage = messageData;
            _overlay.Show();
        }

        public void Update(GameTime time)
        {
            _overlay.Update(time);
        }

        public void Draw()
        {
            _overlay.Draw(_spriteBatch);
        }
    }
}
