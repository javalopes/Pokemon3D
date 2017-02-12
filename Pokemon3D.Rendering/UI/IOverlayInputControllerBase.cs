using System;

namespace Pokemon3D.Rendering.UI
{
    public abstract class IOverlayInputControllerBase : IOverlayInputController
    {
        protected void InvokeMoveToNextElement()
        {
            MoveToNextElement?.Invoke();
        }

        protected void InvokeMoveToPreviousElement()
        {
            MoveToPreviousElement?.Invoke();
        }

        protected void InvokeAction(UiElement element)
        {
            OnAction?.Invoke(element);
        }

        protected void InvokeMoveToElement(UiElement element)
        {
            MoveToElement?.Invoke(element);
        }

        public abstract void Update(UiFocusContainer container);

        public event Action<UiElement> OnAction;
        public event Action<UiElement> MoveToElement;
        public event Action MoveToNextElement;
        public event Action MoveToPreviousElement;
    }
}