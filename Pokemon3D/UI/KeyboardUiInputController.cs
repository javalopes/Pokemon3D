using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    class KeyboardUiInputController : OverlayInputControllerBase
    {
        public override void Update(UiOverlay overlay)
        {
            if (GameProvider.GameInstance.InputSystem.Up(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
            {
                InvokeMoveToPreviousElement();
            }
            if (GameProvider.GameInstance.InputSystem.Down(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
            {
                InvokeMoveToNextElement();
            }
        }
    }
}
