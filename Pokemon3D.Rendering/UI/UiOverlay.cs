using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI
{
    public class UiOverlay
    {
        private readonly List<UiElement> _uiElements;
        private readonly List<UiElement> _interactableElements;
        private readonly List<UiElementContainer> _container; 
        private readonly List<OverlayInputController> _inputControllers;
        private bool _isUiElementListSortedByTabIndex;

        public UiElement CurrentElement { get; private set; }

        public UiOverlay()
        {
            _uiElements = new List<UiElement>();
            _interactableElements = new List<UiElement>();
            _container = new List<UiElementContainer>();
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
                if (uiElement.IsInteractable)
                {
                    uiElement.TabIndex = currentTabIndex;
                    currentTabIndex++;
                }
                else
                {
                    uiElement.TabIndex = -1;
                }
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
            CurrentElement?.OnAction();
        }

        private void ControllerOnMoveToNextElement()
        {
            var lastElement = CurrentElement;
            if (CurrentElement == null)
            {
                CurrentElement = _interactableElements.FirstOrDefault();
            }
            else
            {
                var index = _interactableElements.IndexOf(CurrentElement);
                CurrentElement = index == _interactableElements.Count - 1 ? _interactableElements[0] : _interactableElements[index + 1];
            }

            if (lastElement != CurrentElement)
            {
                lastElement?.Unfocus();
                CurrentElement?.Focus();
            }
        }

        public TElement AddElement<TElement>(TElement element) where TElement : UiElement
        {
            _uiElements.Add(element);
            _isUiElementListSortedByTabIndex = false;
            return element;
        }

        public TContainer AddElementContainer<TContainer>(TContainer container) where TContainer : UiElementContainer
        {
            _container.Add(container);
            return container;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isUiElementListSortedByTabIndex)
            {
                _interactableElements.Clear();
                _interactableElements.AddRange(_uiElements.Where(e => e.IsInteractable).OrderBy(e => e.TabIndex));
                _isUiElementListSortedByTabIndex = true;
            }

            for (var i = 0; i < _uiElements.Count; i++)
            {
                _uiElements[i].Update(gameTime);
            }

            foreach (var container in _container) container.Update(gameTime);

            foreach (var inputController in _inputControllers)
            {
                inputController.Update(this);
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
