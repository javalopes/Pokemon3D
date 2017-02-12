using System;

namespace Pokemon3D.Rendering.UI
{
    /// <summary>
    /// Defining how to control the elements.
    /// </summary>
    public interface IOverlayInputController
    {
        /// <summary>
        /// Called once per frame.
        /// </summary>
        void Update(UiFocusContainer container);

        /// <summary>
        /// This action should be called when an element should execute it's clicked action.
        /// </summary>
        event Action<UiElement> OnAction;

        event Action<UiElement> MoveToElement;

        /// <summary>
        /// Fire this event when to move to the next element by tab index.
        /// </summary>
        event Action MoveToNextElement;

        /// <summary>
        /// Fire this event when to move to the previous element by tab index.
        /// </summary>
        event Action MoveToPreviousElement;
    }
}
