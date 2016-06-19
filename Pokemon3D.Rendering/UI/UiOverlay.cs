using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay
    {
        private readonly List<UiBaseElement> _uiElements;

        public UiOverlay()
        {
            _uiElements = new List<UiBaseElement>();
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiBaseElement
        {
            _uiElements.Add(element);
            return element;
        }

        public void Update(GameTime gameTime)
        {
            for (var i = 0; i < _uiElements.Count; i++)
            {
                _uiElements[i].Update(gameTime);
            }
        }

        public void Show()
        {
            _uiElements.ForEach(e => e.Show());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (var i = 0; i < _uiElements.Count; i++)
            {
                var uiBaseElement = _uiElements[i];
                if (uiBaseElement.State != UiState.Inactive) uiBaseElement.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
