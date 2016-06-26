using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// A default control group which manages moving between controls.
    /// </summary>
    class DefaultControlGroup2 : UiElementContainer
    {
        private UiElement _selectedElement;

        public ControlGroupOrientation Orientation { get; set; } = ControlGroupOrientation.Vertical;

        public override void Focus()
        {
        }

        public override void Unfocus()
        {
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (GameProvider.GameInstance.InputSystem.Up(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                if (Orientation.HasFlag(ControlGroupOrientation.Vertical)) MoveSelection(-1);
            if (GameProvider.GameInstance.InputSystem.Down(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                if (Orientation.HasFlag(ControlGroupOrientation.Vertical)) MoveSelection(1);
            if (GameProvider.GameInstance.InputSystem.Left(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                if (Orientation.HasFlag(ControlGroupOrientation.Horizontal)) MoveSelection(-1);
            if (GameProvider.GameInstance.InputSystem.Right(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                if (Orientation.HasFlag(ControlGroupOrientation.Horizontal)) MoveSelection(1);
        }

        /// <summary>
        /// Moves the selection index by a certain amount (negative to move up).
        /// </summary>
        public void MoveSelection(int change)
        {
            if (_selectedElement == null)
            {
                _selectedElement = Elements.First();
                _selectedElement.Focus();
            }
            else
            {
                var currentIndex = Elements.IndexOf(_selectedElement);
                var previousIndex = currentIndex;

                currentIndex += change;
                while (currentIndex < 0)
                {
                        currentIndex += Elements.Count;
                }
                while (currentIndex >= Elements.Count)
                {
                        currentIndex -= Elements.Count;
                }

                if (previousIndex != currentIndex)
                {
                    var newElement = Elements[currentIndex];

                    if (newElement != _selectedElement)
                    {
                        _selectedElement.Unfocus();
                        _selectedElement = newElement;
                        _selectedElement.Focus();
                    }
                }
            }
        }
    }
}