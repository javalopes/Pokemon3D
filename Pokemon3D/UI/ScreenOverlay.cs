using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.UI
{
    internal class ScreenOverlay
    {
        private readonly object _lockObject = new object();
        private readonly List<UiOverlay> _overlays = new List<UiOverlay>();
        private readonly SpriteBatch _spriteBatch;

        public ScreenOverlay()
        {
            _spriteBatch = GameInstance.GetService<SpriteBatch>();
        }

        public void AddOverlay(UiOverlay overlay)
        {
            lock (_lockObject)
            {
                _overlays.Add(overlay);
            }
        }

        public void RemoveOverlay(UiOverlay overlay)
        {
            lock (_lockObject)
            {
                _overlays.Remove(overlay);
            }
        }

        public void Update(GameTime gameTime)
        {
            lock (_lockObject)
            {
                _overlays.ForEach(o => o.Update(gameTime));
            }
        }

        public void Draw()
        {
            lock (_lockObject)
            {
                _overlays.ForEach(o => o.Draw(_spriteBatch));
            }
        }
    }
}
