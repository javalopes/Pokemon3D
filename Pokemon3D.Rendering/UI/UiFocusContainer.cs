using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI
{
    public class UiFocusContainer
    {
        private readonly List<UiElement> _interactableElements;
        protected readonly List<IOverlayInputController> InputControllers;
        private bool _rearrangeList;
        private UiElement _elementToGetFocus;
        private UiElement _currentElement;

        public UiElement CurrentElement
        {
            get { return _currentElement; }
            private set
            {
                if (_elementToGetFocus != null && _elementToGetFocus != _currentElement) _elementToGetFocus = null;
                _currentElement = value;
            }
        }

        public List<UiElement> UiElements => _interactableElements;

        public UiFocusContainer()
        {
            _interactableElements = new List<UiElement>();
            InputControllers = new List<IOverlayInputController>();
            CurrentElement = null;
        }

        public void FocusFirstElement()
        {
            var firstElement = _interactableElements.FirstOrDefault();
            if (firstElement == CurrentElement) return;

            CurrentElement?.Unfocus();

            _elementToGetFocus = firstElement;
        }

        public void AddInputController(IOverlayInputController controller)
        {
            controller.MoveToNextElement += ControllerOnMoveToNextElement;
            controller.MoveToPreviousElement += ControllerOnMoveToPreviousElement;
            controller.OnAction += ControllerOnOnAction;
            controller.MoveToElement += ControllerOnMoveToElement;

            InputControllers.Add(controller);
        }

        private void ControllerOnMoveToElement(UiElement uiElement)
        {
            if (CurrentElement != uiElement)
            {
                CurrentElement?.Unfocus();
                CurrentElement = uiElement;
                CurrentElement?.Focus();
            }
        }

        public void ClearInputControllers()
        {
            InputControllers.Clear();
        }

        public void SetUiElements(IEnumerable<UiElement> uiElementsToManage, bool autoenumerate)
        {
            _interactableElements.Clear();
            _interactableElements.AddRange(uiElementsToManage.Where(u => u.IsInteractable));

            if (autoenumerate)
            {
                AutoEnumerateTabIndices();
            }
            else
            {
                _interactableElements.Sort((e1,e2) => e1.TabIndex.CompareTo(e2.TabIndex));
            }

            _rearrangeList = false;
        }
        
        protected void AddUiElement(UiElement element)
        {
            _interactableElements.Add(element);
            _rearrangeList = true;
        }

        public void AutoEnumerateTabIndices()
        {
            var currentTabIndex = 0;
            foreach (var uiElement in _interactableElements)
            {
                uiElement.TabIndex = currentTabIndex++;
            }
            _rearrangeList = false;
        }

        public void ControllerOnMoveToPreviousElement()
        {
            var lastElement = CurrentElement;
            if (CurrentElement == null)
            {
                CurrentElement = _interactableElements.FirstOrDefault();
            }
            else
            {
                var index = _interactableElements.IndexOf(CurrentElement);
                CurrentElement = index == 0 ? _interactableElements[_interactableElements.Count - 1] : _interactableElements[index - 1];
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
        
        public virtual void Update(GameTime gameTime)
        {
            if (_rearrangeList)
            {
                _interactableElements.RemoveAll(e => !e.IsInteractable);
                _interactableElements.Sort((e1, e2) => e1.TabIndex.CompareTo(e2.TabIndex));
            }

            foreach (var inputController in InputControllers)
            {
                inputController.Update(this);
            }

            if (_elementToGetFocus != null && _elementToGetFocus.State == UiState.Active)
            {
                _elementToGetFocus.Focus();
                CurrentElement = _elementToGetFocus;
                _elementToGetFocus = null;
            }
        }
    }
}
