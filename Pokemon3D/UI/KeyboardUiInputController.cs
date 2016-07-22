using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    class KeyboardUiInputController : OverlayInputControllerBase
    {
        private InputSystem _inputSystem;

        public KeyboardUiInputController()
        {
            _inputSystem = GameProvider.GameInstance.GetService<InputSystem>();
        }

        public override void Update(UiFocusContainer container)
        {

            if (_inputSystem.Up(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
            {
                InvokeMoveToPreviousElement();
            }
            if (_inputSystem.Down(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
            {
                InvokeMoveToNextElement();
            }
            if (_inputSystem.Accept(AcceptInputTypes.Buttons))
            {
                InvokeAction(container.CurrentElement);
            }
        }
    }
}
