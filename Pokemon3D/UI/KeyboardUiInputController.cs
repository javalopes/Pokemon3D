using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    internal class KeyboardUiInputController : OverlayInputControllerBase
    {
        private readonly InputSystem _inputSystem;

        public KeyboardUiInputController()
        {
            _inputSystem = GameProvider.GameInstance.GetService<InputSystem>();
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
