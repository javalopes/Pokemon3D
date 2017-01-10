using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// ReSharper disable ForCanBeConvertedToForeach
namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay : UiFocusContainer
    {
        private UiCompoundElement _currentModalElement;
        private readonly List<UiElement> _uiElements;
        private bool _waitForShowed;
        private bool _waitForHidden;

        /// <summary>
        /// Is called after show() and all elements have state active.
        /// </summary>
        public event Action Showed;

        /// <summary>
        /// Is called after show() and all elements have state inactive.
        /// </summary>
        public event Action Hidden;

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

            if (_waitForShowed && _uiElements.All(e => e.State == UiState.Active))
            {
                _waitForShowed = false;
                Showed?.Invoke();
            }

            if (_waitForHidden && _uiElements.All(e => e.State == UiState.Inactive))
            {
                _waitForHidden = false;
                Hidden?.Invoke();
            }
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
            _waitForHidden = false;
            _waitForShowed = true;
        }

        public void Hide()
        {
            _uiElements.ForEach(e => e.Hide());
            _waitForHidden = true;
            _waitForShowed = false;
        }

        protected void ApplyOffset(Vector2 offset)
        {
            _uiElements.ForEach(u => u.Offset = offset);
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
