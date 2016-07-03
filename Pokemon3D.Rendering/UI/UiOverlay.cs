using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay : UiFocusContainer
    {
        private UiCompoundElement _currentModalElement;
        private readonly List<UiElement> _uiElements;

        public UiOverlay()
        {
            _uiElements = new List<UiElement>();
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiElement
        {
            _uiElements.Add(element);
            AddUiElement(element);
            return element;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (var i = 0; i < _uiElements.Count; i++)
            {
                _uiElements[i].Update(gameTime);
            }
            _currentModalElement?.Update(gameTime);
        }

        public void ShowModal(UiCompoundElement modalElement)
        {
            _currentModalElement = modalElement;
            SetUiElements(_currentModalElement.Children, true);
            FocusFirstElement();
            _currentModalElement.Show();
        }

        public void CloseModal()
        {
            _currentModalElement?.Hide();
            SetUiElements(_uiElements, false);
            FocusFirstElement();
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

            if (_currentModalElement != null && _currentModalElement.State != UiState.Inactive) _currentModalElement?.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
