using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Content;
using Pokemon3D.Entities.Components;
using Pokemon3D.GameCore;
using static GameProvider;

namespace Pokemon3D.Entities
{
    internal class Player
    {
        private readonly PlayerControllerComponent _controllerComponent;

        public Camera Camera { get; private set; }

        public Player(World world)
        {
            var playerEntity = world.EntitySystem.CreateEntity(true);
            var collisionSize = new Vector3(0.35f, 0.6f, 0.35f);
            var collisionOffset = new Vector3(0.0f, 0.3f, 0.0f);
            var mesh = new Mesh(GameInstance.GraphicsDevice, Primitives.GenerateQuadForYBillboard());
            var diffuseTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.DefaultGuy);
            var material = new Material
            {
                DiffuseTexture = diffuseTexture,
                UseTransparency = true,
                TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                IsUnlit = true
            };
            
            playerEntity.AddComponent(new ModelEntityComponent(playerEntity, mesh, material, true));
            playerEntity.AddComponent(new FigureMovementAnimationComponent(playerEntity, diffuseTexture));
            playerEntity.AddComponent(new CollisionEntityComponent(playerEntity, collisionSize, collisionOffset, "Player", true));
            playerEntity.Position = new Vector3(5, 0, 8);

            var cameraEntity = world.EntitySystem.CreateEntity(true);
            cameraEntity.SetParent(playerEntity);
            _controllerComponent = cameraEntity.AddComponent(new PlayerControllerComponent(cameraEntity));
            var cameraComponent = cameraEntity.AddComponent(new CameraEntityComponent(cameraEntity, new Skybox(GameInstance)
            {
                Scale = 50,
                Texture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.skybox_texture)
            }));
            cameraComponent.FarClipDistance = 50.0f;
            Camera = cameraComponent.Camera;

            var overlayCamera = cameraEntity.AddComponent(new CameraEntityComponent(cameraEntity, null, CameraMasks.UiOverlays));
            overlayCamera.Camera.FarClipDistance = 100.0f;
            overlayCamera.Camera.ClearColor = null;
            overlayCamera.Camera.DepthClear = 1.0f;
            overlayCamera.Camera.DepthStencilState = DepthStencilState.Default;
        }

        public PlayerMovementMode MovementMode
        {
            get { return _controllerComponent.MovementMode; }
            set { _controllerComponent.MovementMode = value; }
        }
    }
}
