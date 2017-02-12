using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI.Controller
{
    internal class MouseUiInputController : OverlayInputControllerBase
    {
        protected readonly InputSystem.InputSystem InputSystem;

        public MouseUiInputController()
        {
            InputSystem = GameProvider.GameInstance.GetService<InputSystem.InputSystem>();
        }

        protected virtual Point GetCurrentMousePosition()
        {
            return new Point(InputSystem.MouseHandler.X, InputSystem.MouseHandler.Y);
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
                        if (InputSystem.MouseHandler.IsLeftButtonDownOnce()) InvokeAction(uiElement);
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