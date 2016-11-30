using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Input;
using Pokemon3D.Entities.System;
using Pokemon3D.Entities.System.Components;

namespace Pokemon3D.Entities.Components
{
    internal class PlayerControllerComponent : EntityComponent
    {
        private MouseState _mouseState;
        private PlayerMovementMode _movementMode;
        private Vector3 _cameraTargetPosition = new Vector3(0, 1, 3);

        public float Speed { get; set; }
        public float RotationSpeed { get; set; }

        public PlayerMovementMode MovementMode
        {
            get { return _movementMode; }
            set
            {
                if(_movementMode == value) return;
                _movementMode = value;
                OnMovementModeChanged();
            }
        }

        public PlayerControllerComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            throw new InvalidOperationException();
        }

        public PlayerControllerComponent(Entity referringEntity)
            : base(referringEntity)
        {
            _movementMode = PlayerMovementMode.ThirdPerson;
            referringEntity.Position = _cameraTargetPosition;
            Speed = 2.0f;
            RotationSpeed = 2f;
        }

        public override EntityComponent Clone(Entity target)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            var inputSystem = GameProvider.GameInstance.GetService<InputSystem>();
            var currentMouseState = Mouse.GetState();

            var movementDirection = Vector3.Zero;
            if (inputSystem.Left(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.X = -1.0f;
            }
            else if (inputSystem.Right(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.X = 1.0f;
            }

            if (inputSystem.Up(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.Z = 1.0f;
            }
            else if (inputSystem.Down(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.Z = -1.0f;
            }

            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    HandleFirstPersonMovement(gameTime.GetSeconds(), currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    HandleThirdPersonMovement(gameTime.GetSeconds(), currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.GodMode:
                    HandleGodModeMovement(gameTime.GetSeconds(), currentMouseState, movementDirection);
                    break;
            }

            _mouseState = currentMouseState;

            if (MovementMode != PlayerMovementMode.GodMode)
            {
                if (Math.Abs(_cameraTargetPosition.Z - ReferringEntity.Position.Z) > float.Epsilon)
                {
                    ReferringEntity.Position = new Vector3(ReferringEntity.Position.X, ReferringEntity.Position.Y, MathHelper.SmoothStep(ReferringEntity.Position.Z, _cameraTargetPosition.Z, 0.2f));
                }
                if (Math.Abs(_cameraTargetPosition.Y - ReferringEntity.Position.Y) > float.Epsilon)
                {
                    ReferringEntity.Position = new Vector3(ReferringEntity.Position.X, MathHelper.SmoothStep(ReferringEntity.Position.Y, _cameraTargetPosition.Y, 0.2f), ReferringEntity.Position.Z);
                }
            }
        }

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var inputSystem = GameProvider.GameInstance.GetService<InputSystem>();

            var speedFactor = inputSystem.Keyboard.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            var step = Speed * elapsedTime * speedFactor;

            if (_mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                var differenceX = mouseState.X - _mouseState.X;
                var differenceY = mouseState.Y - _mouseState.Y;

                ReferringEntity.RotateX(-differenceY * 0.1f * elapsedTime);
                ReferringEntity.RotateY(-differenceX * 0.1f * elapsedTime);
            }

            if (movementDirection.LengthSquared() > 0.0f)
            {
                ReferringEntity.Translate(Vector3.Normalize(movementDirection) * step);
            }
            if (inputSystem.Keyboard.IsKeyDown(Keys.Space))
            {
                ReferringEntity.Position += Vector3.UnitY * step;
            }
        }

        private void HandleThirdPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var inputSystem = GameProvider.GameInstance.GetService<InputSystem>();

            if (movementDirection.LengthSquared() > 0.0f)
            {
                ReferringEntity.Parent.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);
            }

            if (inputSystem.Left(InputDetectionType.HeldDown, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.RightThumbstick))
            {
                ReferringEntity.Parent.RotateY(RotationSpeed * elapsedTime);
            }
            else if (inputSystem.Right(InputDetectionType.HeldDown, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.RightThumbstick))
            {
                ReferringEntity.Parent.RotateY(-RotationSpeed * elapsedTime);
            }
        }

        private void HandleFirstPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var inputSystem = GameProvider.GameInstance.GetService<InputSystem>();

            if (movementDirection.LengthSquared() > 0.0f)
            {
                ReferringEntity.Parent.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);
            }

            if (inputSystem.Keyboard.IsKeyDown(Keys.Left))
            {
                ReferringEntity.Parent.RotateY(RotationSpeed * elapsedTime);
            }
            else if (inputSystem.Keyboard.IsKeyDown(Keys.Right))
            {
                ReferringEntity.Parent.RotateY(-RotationSpeed * elapsedTime);
            }
            if (inputSystem.Keyboard.IsKeyDown(Keys.Up))
            {
                ReferringEntity.RotateX(RotationSpeed * elapsedTime);
            }
            else if (inputSystem.Keyboard.IsKeyDown(Keys.Down))
            {
                ReferringEntity.RotateX(-RotationSpeed * elapsedTime);
            }
        }

        private void OnMovementModeChanged()
        {
            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    ReferringEntity.Parent.GetComponent<ModelEntityComponent>().IsActive = true;
                    ReferringEntity.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 0.6f, 0);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    ReferringEntity.Parent.GetComponent<ModelEntityComponent>().IsActive = false;
                    ReferringEntity.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 1, 3);
                    break;
                case PlayerMovementMode.GodMode:
                    ReferringEntity.Parent.GetComponent<ModelEntityComponent>().IsActive = false;
                    ReferringEntity.Position = ReferringEntity.Parent.GlobalPosition + new Vector3(0, 1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}