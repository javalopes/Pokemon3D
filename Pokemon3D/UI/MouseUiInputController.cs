using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    internal class MouseUiInputController : OverlayInputControllerBase
    {
        protected readonly InputSystem.InputSystem _inputSystem;

        public MouseUiInputController()
        {
            _inputSystem = GameProvider.GameInstance.GetService<InputSystem.InputSystem>();
        }

        protected virtual Point GetCurrentMousePosition()
        {
            return new Point(_inputSystem.MouseHandler.X, _inputSystem.MouseHandler.Y);
        }
        
        public override void Update(UiFocusContainer container)
        {
            var currentMousePosition = GetCurrentMousePosition();

            foreach (var uiElement in container.UiElements)
            {
                if (uiElement.GetBounds().Contains(currentMousePosition.X, currentMousePosition.Y))
                {
                    if (container.CurrentElement == uiElement)
                    {
                        if (_inputSystem.MouseHandler.IsLeftButtonDownOnce()) InvokeAction(uiElement);
                    }
                    else
                    {
                        InvokeMoveToElement(uiElement);
                    }
                    break;
                }
            }
        }
    }
}