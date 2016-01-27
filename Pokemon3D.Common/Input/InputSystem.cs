using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.Common.Input
{
    public class InputSystem
    {
        public KeyboardHandler Keyboard { get; private set; }
        public GamePadHandler GamePad { get; private set; }
        public MouseHandler Mouse { get; private set; }

        public InputSystem()
        {
            Keyboard = new KeyboardHandler();
            GamePad = new GamePadHandler();
            Mouse = new MouseHandler();

            _pressedDirectionDelay = FULL_PRESSED_DIRECTION_DELAY;
        }

        public void Update()
        {
            Keyboard.Update();
            GamePad.Update();
            Mouse.Update();
        }

        private const float FULL_PRESSED_DIRECTION_DELAY = 4.0f;
        private const float RESET_PRESSED_DIRECTION_DELAY = 0.4f;

        private float _pressedDirectionDelay;
        private InputDirection _lastPressedDirection = InputDirection.None;

        private void ChangePressedDirection(InputDirection newDirection)
        {
            if (newDirection != _lastPressedDirection)
            {
                _pressedDirectionDelay = FULL_PRESSED_DIRECTION_DELAY;
                _lastPressedDirection = newDirection;
            }
        }

        private void ResetPressedDirection(InputDirection direction)
        {
            if (direction == _lastPressedDirection)
            {
                _pressedDirectionDelay = FULL_PRESSED_DIRECTION_DELAY;
                _lastPressedDirection = InputDirection.None;
            }
        }

        private bool HoldDownPress(InputDirection direction)
        {
            // returns true if there is a hold down press for this frame.

            if (_lastPressedDirection == direction)
            {
                _pressedDirectionDelay -= 0.1f;
                if (_pressedDirectionDelay <= 0.0f)
                {
                    _pressedDirectionDelay = RESET_PRESSED_DIRECTION_DELAY;
                    return true;
                }
            }
            return false;
        }

        private bool CheckDirectional(InputDetectionType detectionType, InputDirection direction, DirectionalInputTypes inputTypes,
                                             Keys arrowKey, Keys WASDKey, Buttons leftThumbstick, Buttons rightThumbstick, Buttons dPadDirection)
        {
            switch (detectionType)
            {
                case InputDetectionType.PressedOnce:
                    return CheckDirectionalPressedOnce(direction, inputTypes, arrowKey, WASDKey, leftThumbstick, rightThumbstick, dPadDirection);
                case InputDetectionType.HeldDown:
                    return CheckDirectionalHeldDown(inputTypes, arrowKey, WASDKey, leftThumbstick, rightThumbstick, dPadDirection);
                case InputDetectionType.Released:
                    return CheckDirectionalReleased(inputTypes, arrowKey, WASDKey, leftThumbstick, rightThumbstick, dPadDirection);
            }
            return false;
        }

        private bool CheckDirectionalPressedOnce(InputDirection direction, DirectionalInputTypes inputTypes,
                                             Keys arrowKey, Keys WASDKey, Buttons leftThumbstick, Buttons rightThumbstick, Buttons dPadDirection)
        {
            bool hasAnyCommand = false;

            if (inputTypes.HasFlag(DirectionalInputTypes.WASD))
            {
                if (Keyboard.IsKeyDown(WASDKey))
                {
                    hasAnyCommand = true;
                    if (HoldDownPress(direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (Keyboard.IsKeyDownOnce(WASDKey))
                        {
                            ChangePressedDirection(direction);
                            return true;
                        }
                    }
                }
            }
            if (inputTypes.HasFlag(DirectionalInputTypes.ArrowKeys))
            {
                if (Keyboard.IsKeyDown(arrowKey))
                {
                    hasAnyCommand = true;
                    if (HoldDownPress(direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (Keyboard.IsKeyDownOnce(arrowKey))
                        {
                            ChangePressedDirection(direction);
                            return true;
                        }
                    }
                }
            }
            if (inputTypes.HasFlag(DirectionalInputTypes.LeftThumbstick))
            {
                if (GamePad.IsButtonDown(leftThumbstick))
                {
                    hasAnyCommand = true;
                    if (HoldDownPress(direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePad.IsButtonDownOnce(leftThumbstick))
                        {
                            ChangePressedDirection(direction);
                            return true;
                        }
                    }
                }
            }
            if (inputTypes.HasFlag(DirectionalInputTypes.RightThumbstick))
            {
                if (GamePad.IsButtonDown(rightThumbstick))
                {
                    hasAnyCommand = true;
                    if (HoldDownPress(direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePad.IsButtonDownOnce(rightThumbstick))
                        {
                            ChangePressedDirection(direction);
                            return true;
                        }
                    }
                }
            }
            if (inputTypes.HasFlag(DirectionalInputTypes.DPad))
            {
                if (GamePad.IsButtonDown(dPadDirection))
                {
                    hasAnyCommand = true;
                    if (HoldDownPress(direction))
                    {
                        return true;
                    }
                    else
                    {
                        if (GamePad.IsButtonDownOnce(dPadDirection))
                        {
                            ChangePressedDirection(direction);
                            return true;
                        }
                    }
                }
            }

            if (!hasAnyCommand)
                ResetPressedDirection(direction);

            return false;
        }

        private bool CheckDirectionalHeldDown(DirectionalInputTypes inputTypes,
                                          Keys arrowKey, Keys WASDKey, Buttons leftThumbstick, Buttons rightThumbstick, Buttons dPadDirection)
        {
            if (inputTypes.HasFlag(DirectionalInputTypes.ArrowKeys))
                if (Keyboard.IsKeyDown(arrowKey)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.WASD))
                if (Keyboard.IsKeyDown(WASDKey)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.LeftThumbstick))
                if (GamePad.IsButtonDown(leftThumbstick)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.RightThumbstick))
                if (GamePad.IsButtonDown(rightThumbstick)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.DPad))
                if (GamePad.IsButtonDown(dPadDirection)) return true;

            return false;
        }

        private bool CheckDirectionalReleased(DirectionalInputTypes inputTypes,
                                          Keys arrowKey, Keys WASDKey, Buttons leftThumbstick, Buttons rightThumbstick, Buttons dPadDirection)
        {
            if (inputTypes.HasFlag(DirectionalInputTypes.ArrowKeys))
                if (Keyboard.IsKeyUp(arrowKey)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.WASD))
                if (Keyboard.IsKeyUp(WASDKey)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.LeftThumbstick))
                if (GamePad.IsButtonUp(leftThumbstick)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.RightThumbstick))
                if (GamePad.IsButtonUp(rightThumbstick)) return true;
            if (inputTypes.HasFlag(DirectionalInputTypes.DPad))
                if (GamePad.IsButtonUp(dPadDirection)) return true;

            return false;
        }

        public bool Left(InputDetectionType detectionType, DirectionalInputTypes inputTypes)
        {
            // todo: for all directions: check if game is active!

            if (inputTypes.HasFlag(DirectionalInputTypes.ScrollWheel) && Mouse.GetScrollWheelDifference() > 0)
            {
                return true;
            }
            else if (inputTypes != DirectionalInputTypes.ScrollWheel) // when it's only scroll wheel, do not check the rest.
            {
                return CheckDirectional(detectionType, InputDirection.Left, inputTypes, Keys.Left, Keys.A, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft, Buttons.DPadLeft);
            }
            return false;
        }
        public bool Right(InputDetectionType detectionType, DirectionalInputTypes inputTypes)
        {
            // todo: for all directions: check if game is active!

            if (inputTypes.HasFlag(DirectionalInputTypes.ScrollWheel) && Mouse.GetScrollWheelDifference() < 0)
            {
                return true;
            }
            else if (inputTypes != DirectionalInputTypes.ScrollWheel) // when it's only scroll wheel, do not check the rest.
            {
                return CheckDirectional(detectionType, InputDirection.Right, inputTypes, Keys.Right, Keys.D, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight, Buttons.DPadRight);
            }
            return false;
        }
        public bool Up(InputDetectionType detectionType, DirectionalInputTypes inputTypes)
        {
            // todo: for all directions: check if game is active!

            if (inputTypes.HasFlag(DirectionalInputTypes.ScrollWheel) && Mouse.GetScrollWheelDifference() > 0)
            {
                return true;
            }
            else if (inputTypes != DirectionalInputTypes.ScrollWheel) // when it's only scroll wheel, do not check the rest.
            {
                return CheckDirectional(detectionType, InputDirection.Up, inputTypes, Keys.Up, Keys.W, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp, Buttons.DPadUp);
            }
            return false;
        }
        public bool Down(InputDetectionType detectionType, DirectionalInputTypes inputTypes)
        {
            // todo: for all directions: check if game is active!

            if (inputTypes.HasFlag(DirectionalInputTypes.ScrollWheel) && Mouse.GetScrollWheelDifference() < 0)
            {
                return true;
            }
            else if (inputTypes != DirectionalInputTypes.ScrollWheel) // when it's only scroll wheel, do not check the rest.
            {
                return CheckDirectional(detectionType, InputDirection.Down, inputTypes, Keys.Down, Keys.S, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown, Buttons.DPadDown);
            }
            return false;
        }

        public bool Accept(AcceptInputTypes inputTypes)
        {
            if (inputTypes.HasFlag(AcceptInputTypes.EnterKey))
            {
                if (Keyboard.IsKeyDownOnce(Keys.Enter))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(AcceptInputTypes.SpacebarKey))
            {
                if (Keyboard.IsKeyDownOnce(Keys.Space))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(AcceptInputTypes.LeftClick))
            {
                if (Mouse.IsButtonDownOnce(MouseButton.Left))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(AcceptInputTypes.AButton))
            {
                if (GamePad.IsButtonDownOnce(Buttons.A))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Dismiss(DismissInputTypes inputTypes)
        {
            if (inputTypes.HasFlag(DismissInputTypes.EscapeKey))
            {
                if (Keyboard.IsKeyDownOnce(Keys.Escape))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(DismissInputTypes.EKey))
            {
                if (Keyboard.IsKeyDownOnce(Keys.E))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(DismissInputTypes.RightClick))
            {
                if (Mouse.IsButtonDownOnce(MouseButton.Right))
                {
                    return true;
                }
            }
            if (inputTypes.HasFlag(DismissInputTypes.BButton))
            {
                if (GamePad.IsButtonDownOnce(Buttons.B))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
