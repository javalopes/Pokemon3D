using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using Pokemon3D.Entities.System.Components;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using Pokemon3D.Content;
using Pokemon3D.Entities.Components;
using Pokemon3D.GameCore;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Entities
{
    internal class Player
    {
        private readonly PlayerControllerComponent _controllerComponent;
        private readonly CameraEntityComponent _mainCameraComponent;

        public Player(World world)
        {
            var playerEntity = world.EntitySystem.CreateEntity(true);
            var collisionSize = new Vector3(0.35f, 0.6f, 0.35f);
            var collisionOffset = new Vector3(0.0f, 0.3f, 0.0f);
            var mesh = new Mesh(GameInstance.GetService<GraphicsDevice>(), Primitives.GenerateQuadForYBillboard());
            var diffuseTexture = GameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.DefaultGuy);
            var material = new Material
            {
                DiffuseTexture = diffuseTexture,
                UseTransparency = true,
                TexcoordScale = diffuseTexture.GetTexcoordsFromPixelCoords(32, 32),
                IsUnlit = true,
                ReceiveShadow = false
            };
            
            playerEntity.AddComponent(new ModelEntityComponent(playerEntity, mesh, material, true));
            playerEntity.AddComponent(new FigureMovementAnimationComponent(playerEntity, diffuseTexture));
            playerEntity.AddComponent(new CollisionEntityComponent(playerEntity, collisionSize, collisionOffset, "Player", true));
            playerEntity.Position = new Vector3(5, 0, 8);

            var cameraEntity = world.EntitySystem.CreateEntity(true);
            cameraEntity.Id = "MainCamera";
            cameraEntity.SetParent(playerEntity);
            _controllerComponent = cameraEntity.AddComponent(new PlayerControllerComponent(cameraEntity));
            _mainCameraComponent = cameraEntity.AddComponent(new CameraEntityComponent(cameraEntity, new Skybox(GameInstance.GetService<GraphicsDevice>())
            {
                Scale = 50,
                Texture = GameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.skybox_texture)
            }));
            _mainCameraComponent.FarClipDistance = 50.0f;
            _mainCameraComponent.Camera.IsMain = true;

            var defaultPostProcessors = GameInstance.GetService<SceneRenderer>().DefaultPostProcessors;
            _mainCameraComponent.Camera.PostProcess.Add(defaultPostProcessors.HorizontalBlur);
            _mainCameraComponent.Camera.PostProcess.Add(defaultPostProcessors.VerticalBlur);

            var overlayCamera = cameraEntity.AddComponent(new CameraEntityComponent(cameraEntity, null, CameraMasks.UiOverlays));
            overlayCamera.Camera.FarClipDistance = 100.0f;
            overlayCamera.Camera.ClearColor = null;
            overlayCamera.Camera.DepthClear = 1.0f;
            overlayCamera.Camera.DepthStencilState = DepthStencilState.Default;
            overlayCamera.Camera.UseCulling = false;

            GameInstance.GetService<EventAggregator>().Subscribe<GameEvent>(GameInstanceOnGameEventRaised);
        }

        private void GameInstanceOnGameEventRaised(GameEvent gameEvent)
        {
            if (gameEvent.Category == GameEvent.Inventory)
            {
                _mainCameraComponent.Camera.PostProcess.IsActive = gameEvent.GetProperty<bool>("Open");
            }
        }

        public PlayerMovementMode MovementMode
        {
            get { return _controllerComponent.MovementMode; }
            set { _controllerComponent.MovementMode = value; }
        }
    }
}
