using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Animations;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using System;

namespace Pokemon3D.GameModes.Maps
{
    class Player : Entity
    {
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

        public Player(Scene scene) : base(scene)
        {
            Camera = scene.CreateCamera();
            Camera.SetParent(SceneNode);
            Camera.FarClipDistance = 50.0f;

            Camera.Skybox = new Skybox(Game)
            {
                Scale = 50,
                Texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.skybox_texture)
            };

            Speed = 2.0f;
            RotationSpeed = 2f;

            SceneNode.Mesh = new Mesh(Game.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            var diffuseTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.DefaultGuy);
            SceneNode.Material = new Material
            {
                DiffuseTexture = diffuseTexture,
                UseTransparency = true,
                TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                IsUnlit = true
            };
            SceneNode.Position = new Vector3(10, 1, 8);
            SceneNode.IsBillboard = true;

            _figureAnimator = new Animator();
            _figureAnimator.AddAnimation("WalkForward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 0),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 0),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkLeft", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 32),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 32),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkRight", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 96),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 96),
            }, t => SceneNode.Material.TexcoordOffset = t, true));
            _figureAnimator.AddAnimation("WalkBackward", Animation.CreateDiscrete(0.65f, new[]
            {
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(32, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(0, 64),
                diffuseTexture.GetTexcoordsFromPixelCoords(64, 64),
            }, t => SceneNode.Material.TexcoordOffset = t, true));

            MovementMode = PlayerMovementMode.ThirdPerson;
            Camera.Position = _cameraTargetPosition;

            Collider = Collisions.Collider.CreateBoundingBox(new Vector3(0.35f,0.6f, 0.35f), new Vector3(0.0f, 0.3f, 0.0f));
        }

        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);

            _figureAnimator.Update(elapsedTime);

            var currentMouseState = Mouse.GetState();

            var movementDirection = Vector3.Zero;
            if (Game.Keyboard.IsKeyDown(Keys.A))
            {
                movementDirection.X = -1.0f;
            }
            else if (Game.Keyboard.IsKeyDown(Keys.D))
            {
                movementDirection.X = 1.0f;
            }

            if (Game.Keyboard.IsKeyDown(Keys.W))
            {
                movementDirection.Z = 1.0f;
            }
            else if (Game.Keyboard.IsKeyDown(Keys.S))
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
                if (_cameraTargetPosition.Z != Camera.Position.Z)
                {
                    Camera.Position = new Vector3(Camera.Position.X, Camera.Position.Y, MathHelper.SmoothStep(Camera.Position.Z, _cameraTargetPosition.Z, 0.2f));
                }
                if (_cameraTargetPosition.Y != Camera.Position.Y)
                {
                    Camera.Position = new Vector3(Camera.Position.X, MathHelper.SmoothStep(Camera.Position.Y, _cameraTargetPosition.Y, 0.2f), Camera.Position.Z);
                }
            }

            Collider.SetPosition(SceneNode.Position);

            var result = Game.CollisionManager.CheckCollision(Collider);
            if (result.Collides)
            {
                SceneNode.Position = SceneNode.Position += result.Axis;
                Collider.SetPosition(SceneNode.Position);
            }
        }

        private void DeactivateWalkingAnimation()
        {
            if (_figureAnimator.CurrentAnimation != null)
            {
                _figureAnimator.Stop();
                SceneNode.Material.TexcoordOffset = Vector2.Zero;
            }
        }

        private void HandleGodModeMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            var speedFactor = Game.Keyboard.IsKeyDown(Keys.LeftShift) ? 2.0f : 1.0f;
            var step = Speed * elapsedTime * speedFactor;

            if (_mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Pressed)
            {
                var differenceX = mouseState.X - _mouseState.X;
                var differenceY = mouseState.Y - _mouseState.Y;

                Camera.RotateX(-differenceY * 0.1f * elapsedTime);
                Camera.RotateY(-differenceX * 0.1f * elapsedTime);
            }

            if (movementDirection.LengthSquared() > 0.0f)
            {
                Camera.Translate(Vector3.Normalize(movementDirection) * step);
            }
            if (Game.Keyboard.IsKeyDown(Keys.Space))
            {
                Camera.Position += Vector3.UnitY * step;
            }
        }

        private void HandleThirdPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            AnimateFigure(elapsedTime, movementDirection);

            if (Game.Keyboard.IsKeyDown(Keys.Left))
            {
                SceneNode.RotateY(RotationSpeed * elapsedTime);
            }
            else if (Game.Keyboard.IsKeyDown(Keys.Right))
            {
                SceneNode.RotateY(-RotationSpeed * elapsedTime);
            }
        }

        private void HandleFirstPersonMovement(float elapsedTime, MouseState mouseState, Vector3 movementDirection)
        {
            AnimateFigure(elapsedTime, movementDirection);

            if (movementDirection.LengthSquared() > 0.0f)
            {
                SceneNode.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);
            }

            if (Game.Keyboard.IsKeyDown(Keys.Left))
            {
                SceneNode.RotateY(RotationSpeed * elapsedTime);
            }
            else if (Game.Keyboard.IsKeyDown(Keys.Right))
            {
                SceneNode.RotateY(-RotationSpeed * elapsedTime);
            }
            if (Game.Keyboard.IsKeyDown(Keys.Up))
            {
                Camera.RotateX(RotationSpeed * elapsedTime);
            }
            else if (Game.Keyboard.IsKeyDown(Keys.Down))
            {
                Camera.RotateX(-RotationSpeed * elapsedTime);
            }
        }

        private void AnimateFigure(float elapsedTime, Vector3 movementDirection)
        {
            if (movementDirection.LengthSquared() > 0.0f)
            {
                SceneNode.Translate(Vector3.Normalize(movementDirection) * Speed * elapsedTime);

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
                    SceneNode.IsActive = true;
                    Camera.SetParent(SceneNode);
                    Camera.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 0.6f, 0);

                    break;
                case PlayerMovementMode.ThirdPerson:
                    SceneNode.IsActive = true;
                    Camera.SetParent(SceneNode);
                    Camera.EulerAngles = Vector3.Zero;
                    _cameraTargetPosition = new Vector3(0, 1, 3);

                    break;
                case PlayerMovementMode.GodMode:
                    Camera.SetParent(null);
                    SceneNode.IsActive = false;
                    Camera.Position = SceneNode.GlobalPosition + new Vector3(0, 1, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
