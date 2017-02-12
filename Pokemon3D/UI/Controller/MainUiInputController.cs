using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI.Controller
{
    internal class MainUiInputController : IOverlayInputControllerBase
    {
        private readonly InputSystem.InputSystem _inputSystem;

        public MainUiInputController()
        {
            _inputSystem = GameProvider.IGameInstance.GetService<InputSystem.InputSystem>();
        }

        public override void Update(UiFocusContainer container)
        {
            if (_inputSystem.IsPressedOnce(ActionNames.MenuUp))
            {
                InvokeMoveToPreviousElement();
            }
            if (_inputSystem.IsPressedOnce(ActionNames.MenuDown))
            {
                InvokeMoveToNextElement();
            }
            if (_inputSystem.IsPressedOnce(ActionNames.MenuAccept))
            {
                InvokeAction(container.CurrentElement);
            }
        }
    }
}
