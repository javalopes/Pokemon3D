using Pokemon3D.Common.Input;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    internal class MouseUiInputController : OverlayInputControllerBase
    {
        private readonly InputSystem _inputSystem;

        public MouseUiInputController()
        {
            _inputSystem = GameProvider.GameInstance.GetService<InputSystem>();
        }

        public override void Update(UiFocusContainer container)
        {
            foreach (var uiElement in container.UiElements)
            {
                if (uiElement.GetBounds().Contains(_inputSystem.MouseHandler.X, _inputSystem.MouseHandler.Y))
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