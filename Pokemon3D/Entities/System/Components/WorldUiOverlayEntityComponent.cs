using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Data;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Entities.System.Components
{
    class WorldUiOverlayEntityComponent : EntityComponent
    {
        private readonly RenderTarget2D _renderTarget;
        private readonly DrawableElement _drawableElement;

        public RenderTarget2D DrawTarget => _renderTarget;
        
        public WorldUiOverlayEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
        }

        public WorldUiOverlayEntityComponent(Entity referringEntity, int width, int height)
            : base(referringEntity)
        {
            var graphicsDevice = GameInstance.GetService<GraphicsDevice>();
            _renderTarget = new RenderTarget2D(graphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);

            _drawableElement = GameInstance.GetService<SceneRenderer>().CreateDrawableElement(true, CameraMasks.UiOverlays);
            _drawableElement.Mesh = new Mesh(graphicsDevice, Primitives.GenerateQuadForY(), false);
            _drawableElement.Material = new Material
            {
                CastShadow = false,
                Color = Color.White,
                DiffuseTexture = _renderTarget,
                ReceiveShadow = false,
                UseTransparency = false,
                IsUnlit = true,
                UseLinearTextureSampling = true
            };
        }

        public override void OnInitialized()
        {
            _drawableElement.EndInitialzing();
        }

        public override void OnComponentRemove()
        {
            GameInstance.GetService<SceneRenderer>().RemoveDrawableElement(_drawableElement);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _drawableElement.WorldMatrix = ReferringEntity.WorldMatrix;
        }

        public override EntityComponent Clone(Entity target)
        {
            throw new InvalidOperationException("Component cannot be cloned");
        }

        public override void OnIsActiveChanged()
        {
            _drawableElement.IsActive = IsActive;
        }
    }
}
