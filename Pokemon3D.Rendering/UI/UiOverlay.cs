using System.Collections.Generic;
using System.Linq;
using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay
    {
        private readonly List<UiBaseElement> _uiBaseElements;
        private readonly List<UiElement> _uiElements;
        private readonly List<OverlayInputController> _inputControllers;
        private bool _isUiElementListSortedByTabIndex;

        public UiElement CurrentElement { get; private set; }

        public UiOverlay()
        {
            _uiBaseElements = new List<UiBaseElement>();
            _uiElements = new List<UiElement>();
            _inputControllers = new List<OverlayInputController>();
            CurrentElement = null;
            _isUiElementListSortedByTabIndex = true;
        }

        public void AddInputController(OverlayInputController controller)
        {
            controller.MoveToNextElement += ControllerOnMoveToNextElement;
            controller.MoveToPreviousElement += ControllerOnMoveToPreviousElement;
            controller.OnAction += ControllerOnOnAction;

            _inputControllers.Add(controller);
        }

        public void AutoEnumerateTabIndices()
        {
            var currentTabIndex = 0;
            foreach (var uiElement in _uiElements)
            {
                uiElement.TabIndex = currentTabIndex;
                currentTabIndex++;
            }
            _isUiElementListSortedByTabIndex = true;
        }

        public void ControllerOnMoveToPreviousElement()
        {
            var lastElement = CurrentElement;
            if (CurrentElement == null)
            {
                CurrentElement = _uiElements.FirstOrDefault();
            }
            else
            {
                var index = _uiElements.IndexOf(CurrentElement);
                CurrentElement = index == 0 ? _uiElements[_uiElements.Count-1] : _uiElements[index - 1];
            }

            if (lastElement != CurrentElement)
            {
                lastElement?.Unfocus();
                CurrentElement?.Focus();
            }
        }

        private void ControllerOnOnAction(UiElement uiElement)
        {
        }

        private void ControllerOnMoveToNextElement()
        {
            var lastElement = CurrentElement;
            if (CurrentElement == null)
            {
                CurrentElement = _uiElements.FirstOrDefault();
            }
            else
            {
                var index = _uiElements.IndexOf(CurrentElement);
                CurrentElement = index == _uiElements.Count - 1 ? _uiElements[0] : _uiElements[index + 1];
            }

            if (lastElement != CurrentElement)
            {
                lastElement?.Unfocus();
                CurrentElement?.Focus();
            }
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiBaseElement
        {
            _uiBaseElements.Add(element);
            var uiElement = element as UiElement;
            if (uiElement != null)
            {
                _uiElements.Add(uiElement);
                _isUiElementListSortedByTabIndex = false;
            }
            return element;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isUiElementListSortedByTabIndex)
            {
                _uiElements.Sort((e1,e2) => e1.TabIndex.CompareTo(e2.TabIndex));
                _isUiElementListSortedByTabIndex = true;
            }

            for (var i = 0; i < _uiBaseElements.Count; i++)
            {
                _uiBaseElements[i].Update(gameTime);
            }

            foreach (var inputController in _inputControllers)
            {
                inputController.Update(this);
            }
        }

        public void Show()
        {
            _uiBaseElements.ForEach(e => e.Show());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (var i = 0; i < _uiBaseElements.Count; i++)
            {
                var uiBaseElement = _uiBaseElements[i];
                if (uiBaseElement.State != UiState.Inactive) uiBaseElement.Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
