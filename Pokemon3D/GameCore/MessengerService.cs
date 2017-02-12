using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.GameCore
{
    internal class MessengerService
    {
        private readonly object _lock = new object();
        private readonly MessageOverlay _overlay;
        private MessageData _activeMessage;
        private readonly SpriteBatch _spriteBatch;

        public MessengerService()
        {
            _overlay = new MessageOverlay();
            
            _overlay.Hidden += OverlayOnHidden;
            _spriteBatch = GameInstance.GetService<SpriteBatch>();
        }

        private void OverlayOnHidden()
        {
            lock(_lock)
            {
                _activeMessage = null;
            }
        }

        public void ShowMessage(MessageData messageData)
        {
            lock (_lock)
            {
                _activeMessage = messageData;
                _overlay.SetMessage(_activeMessage.Text);
                _overlay.Show();
            }
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
