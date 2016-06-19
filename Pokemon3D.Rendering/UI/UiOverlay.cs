using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay
    {
        private readonly List<UiElement> _uiElements;

        public UiOverlay()
        {
            _uiElements = new List<UiElement>();
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiElement
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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (var i = 0; i < _uiElements.Count; i++)
            {
                _uiElements[i].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
