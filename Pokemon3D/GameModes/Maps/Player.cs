using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Common.Input;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes.Maps.EntityComponents.Components;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using System;

namespace Pokemon3D.GameModes.Maps
{
    class Player : GameObject
    {
        private Entity _playerEntity;
        private Entity _cameraEntity;

        private ModelEntityComponent _modelEntityComponent;
        private readonly Animator _figureAnimator;
        private PlayerMovementMode _movementMode;
        private MouseState _mouseState;

        private Vector3 _cameraTargetPosition = new Vector3(0, 1, 3);

        public float Speed { get; set; }
        public float RotationSpeed { get; set; }

        public Camera Camera { get; private set; }

        public PlayerMovementMode MovementMode
        {
            get { return _movementMode; }
            set
            {
                if (_movementMode != value)
                {
                    _movementMode = value;
                    OnMovementModeChanged();
                }
            }
        }

        public Player(EntitySystem entitySystem)
        {
            _playerEntity = entitySystem.CreateEntity();

            var mesh = new Mesh(Game.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            var diffuseTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.DefaultGuy);
            var material = new Material
            {
                DiffuseTexture = diffuseTexture,
                UseTransparency = true,
                TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                IsUnlit = true
            };
            _modelEntityComponent = _playerEntity.AddComponent(new ModelEntityComponent(_playerEntity, mesh, material, true));

            _cameraEntity = entitySystem.CreateEntity(_playerEntity);
            var cameraComponent = _cameraEntity.AddComponent(new CameraEntityComponent(_cameraEntity, new Skybox(Game)
            {
                Scale = 50,
                Texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.skybox_texture)
            }));
            cameraComponent.FarClipDistance = 50.0f;
            Camera = cameraComponent.Camera;

            Speed = 2.0f;
            RotationSpeed = 2f;
            
            _figureAnimator = new Animator();
            _figureAnimator.AddAnimation("WalkForward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 0),
            }, t => _modelEntityComponent.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkLeft", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 32),
            }, t => _modelEntityComponent.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkRight", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 96),
            }, t => _modelEntityComponent.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkBackward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 64),
            }, t => _modelEntityComponent.Material.TexcoordOffset = t, true));

            MovementMode = PlayerMovementMode.ThirdPerson;
            _cameraEntity.Position = _cameraTargetPosition;

            var colliderComponent = new CollisionEntityComponent(_playerEntity, new Vector3(0.35f, 0.6f, 0.35f),
                new Vector3(0.0f, 0.3f, 0.0f))
            {
                ResolvesPosition = true
            };
            _playerEntity.AddComponent(colliderComponent);

            _playerEntity.Position = new Vector3(10,1,8 );
        }

        public void Update(float elapsedTime)
        {
            _figureAnimator.Update(elapsedTime);

            var currentMouseState = Mouse.GetState();

            var movementDirection = Vector3.Zero;
            if (Game.InputSystem.Left(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.X = -1.0f;
            }
            else if (Game.InputSystem.Right(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.X = 1.0f;
            }

            if (Game.InputSystem.Up(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.Z = 1.0f;
            }
            else if (Game.InputSystem.Down(InputDetectionType.HeldDown, DirectionalInputTypes.WASD | DirectionalInputTypes.LeftThumbstick))
            {
                movementDirection.Z = -1.0f;
            }

            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    HandleFirstPersonMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    HandleThirdPersonMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
                case PlayerMovementMode.GodMode:
                    HandleGodModeMovement(elapsedTime, currentMouseState, movementDirection);
                    break;
            }

            _mouseState = currentMouseState;

            if (MovementMode != PlayerMovementMode.GodMode)
            {
                if (_cameraTargetPosition.Z != _cameraEntity.Position.Z)
                {
                    _cameraEntity.Position = new Vector3(_cameraEntity.Position.X, _cameraEntity.Position.Y, MathHelper.SmoothStep(_cameraEntity.Position.Z, _cameraTargetPosition.Z, 0.2f));
                }
                if (_cameraTargetPosition.Y != _cameraEntity.Position.Y)
                {
                    _cameraEntity.Position = new Vector3(_cameraEntity.Position.X, MathHelper.SmoothStep(_cameraEntity.Position.Y, _cameraTargetPosition.Y, 0.2f), _cameraEntity.Position.Z);
                }
            }

            //var collidingObjects = Game.CollisionManager.CheckCollision(Collider);
            // if (collidingObjects.Length > 0)
            //{
            //    for(var i = 0; i < collidingObjects.Length; i++)
            //    {
            //        //SceneNode.Position = SceneNode.Position += collidingObjects[i].Axis;
            //    }
            //    //Collider.SetPosition(SceneNode.Position);
            //}
        }

        private void DeactivateWalkingAnimation()
        {
            if (_figureAnimator.CurrentAnimation != null)
            {
                _figureAnimator.Stop();
                _modelEntityComponent.Material.TexcoordOffset = Vector2.Zero;
            }
        }

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var speedFactor = Game.InputSystem.Keyboard.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            var step = Speed * elapsedTime * speedFactor;

            if (_mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                var differenceX = mouseState.X - _mouseState.X;
                var differenceY = mouseState.Y - _mouseState.Y;

                _cameraEntity.RotateX(-differenceY * 0.1f * elapsedTime);
                _cameraEntity.RotateY(-differenceX * 0.1f * elapsedTime);
            }

            if (movementDirection.LengthSquared() > 0.0f)
            {
                _cameraEntity.Translate(Vector3.Normalize(movementDirection) * step);
            }
            if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Space))
            {
                _cameraEntity.Position += Vector3.UnitY * step;
            }
        }

        private void HandleThirdPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            AnimateFigure(elapsedTime, movementDirection);

            if (Game.InputSystem.Left(InputDetectionType.HeldDown, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.RightThumbstick))
            {
                _playerEntity.RotateY(RotationSpeed * elapsedTime);
            }
            else if (Game.InputSystem.Right(InputDetectionType.HeldDown, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.RightThumbstick))
            {
                _playerEntity.RotateY(-RotationSpeed * elapsedTime);
            }
        }

        private void HandleFirstPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            AnimateFigure(elapsedTime, movementDirection);

            if (movementDirection.LengthSquared() > 0.0f)
            {
                _playerEntity.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);
            }

            if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Left))
            {
                _playerEntity.RotateY(RotationSpeed * elapsedTime);
            }
            else if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Right))
            {
                _playerEntity.RotateY(-RotationSpeed * elapsedTime);
            }
            if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Up))
            {
                _cameraEntity.RotateX(RotationSpeed * elapsedTime);
            }
            else if (Game.InputSystem.Keyboard.IsKeyDown(Keys.Down))
            {
                _cameraEntity.RotateX(-RotationSpeed * elapsedTime);
            }
        }

        private void AnimateFigure(float elapsedTime, Vector3 movementDirection)
        {
            if (movementDirection.LengthSquared() > 0.0f)
            {
                _playerEntity.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);

                if (movementDirection.X > 0.0f)
                {
                    _figureAnimator.SetAnimation("WalkRight");
                }
                else if (movementDirection.X < 0.0f)
                {
                    _figureAnimator.SetAnimation("WalkLeft");
                }
                else
                {
                    if (movementDirection.Z > 0.0f)
                    {
                        _figureAnimator.SetAnimation("WalkForward");
                    }
                    else
                    {
                        _figureAnimator.SetAnimation("WalkBackward");
                    }
                }
            }
            else
            {
                DeactivateWalkingAnimation();
            }
        }

        private void OnMovementModeChanged()
        {
            switch (MovementMode)
            {
                case PlayerMovementMode.FirstPerson:
                    _playerEntity.IsActive = true;

                    _cameraEntity.SetParent(_playerEntity);
                    _cameraEntity.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 0.6f, 0);
                    break;
                case PlayerMovementMode.ThirdPerson:
                    _playerEntity.IsActive = true;
                    _cameraEntity.SetParent(_playerEntity);
                    _cameraEntity.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 1, 3);

                    break;
                case PlayerMovementMode.GodMode:
                    _cameraEntity.SetParent(null);
                    _playerEntity.IsActive = false;
                    _cameraEntity.Position = _playerEntity.GlobalPosition + new Vector3(0, 1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
