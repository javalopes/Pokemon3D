using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Entities.System;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.GameCore;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Entities.Components
{
    internal class PlayerControllerComponent : EntityComponent
    {
        private MouseState _mouseState;
        private PlayerMovementMode _movementMode;
        private Vector3 _cameraTargetPosition = new Vector3(0, 1, 3);
        private bool _isInputEnabled;

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
            _isInputEnabled = true;
            referringEntity.Position = _cameraTargetPosition;
            Speed = 2.0f;
            RotationSpeed = 2f;

            GameInstance.GetService<EventAggregator>().GameEventRaised += OnGameEventRaised;
        }

        public override void OnComponentRemove()
        {
            GameInstance.GetService<EventAggregator>().GameEventRaised -= OnGameEventRaised;
        }

        private void OnGameEventRaised(GameEvent gameEvent)
        {
            if (gameEvent.Name == GameEvent.InventoryOpenend)
            {
                _isInputEnabled = false;
            }
            else if (gameEvent.Name == GameEvent.InventoryClosed)
            {
                _isInputEnabled = true;
            }
        }

        public override EntityComponent Clone(Entity target)
        {
            throw new InvalidOperationException("Component cannot be cloned");
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isInputEnabled) return;

            var inputSystem = GameInstance.GetService<InputSystem.InputSystem>();
            var currentMouseState = Mouse.GetState();

            var movementDirection = inputSystem.GetAxis(ActionNames.LeftAxis);

            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    HandleFirstPersonMovement(gameTime.GetSeconds(), movementDirection);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    HandleThirdPersonMovement(gameTime.GetSeconds(), movementDirection);
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

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector2 movementDirection)
        {
            var inputSystem = GameInstance.GetService<InputSystem.InputSystem>();

            var speedFactor = inputSystem.IsPressed(ActionNames.SprintGodMode) ? 2.0f : 1.0f;
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
                var movementNormalized = Vector2.Normalize(movementDirection);
                ReferringEntity.Translate(new Vector3(movementNormalized.X, 0.0f, movementNormalized.Y) * step);
            }
            if (inputSystem.IsPressed(ActionNames.StraveGodMode))
            {
                ReferringEntity.Position += Vector3.UnitY * step;
            }
        }

        private void HandleThirdPersonMovement(float elapsedTime, Vector2 movementDirection)
        {
            if (movementDirection.LengthSquared() > 0.0f)
            {
                var movementNormalized = Vector2.Normalize(movementDirection);
                ReferringEntity.Parent.Translate(new Vector3(movementNormalized.X, 0.0f, movementNormalized.Y) * Speed * elapsedTime);
            }

            var rightAxis = GameInstance.GetService<InputSystem.InputSystem>().GetAxis(ActionNames.RightAxis);
            if (rightAxis.X * rightAxis.X > 0.0f)
            {
                ReferringEntity.Parent.RotateY(-rightAxis.X * RotationSpeed * elapsedTime);
            }
        }

        private void HandleFirstPersonMovement(float elapsedTime, Vector2 movementDirection)
        {
            if (movementDirection.LengthSquared() > 0.0f)
            {
                var movementNormalized = Vector2.Normalize(movementDirection);
                ReferringEntity.Parent.Translate(new Vector3(movementNormalized.X, 0.0f, movementNormalized.Y) * Speed * elapsedTime);
            }

            var rightAxis = GameInstance.GetService<InputSystem.InputSystem>().GetAxis(ActionNames.RightAxis);

            if (rightAxis.X*rightAxis.X > 0.0f)
            {
                ReferringEntity.Parent.RotateY(rightAxis.X * RotationSpeed * elapsedTime);
            }
            if (rightAxis.Y * rightAxis.Y > 0.0f)
            {
                ReferringEntity.Parent.RotateY(rightAxis.Y * RotationSpeed * elapsedTime);
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