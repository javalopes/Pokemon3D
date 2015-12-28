﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering.Compositor
{
    class ForwardSceneRenderer : GameContextObject, SceneRenderer
    {
        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private RenderTargetBinding[] _oldBindings;

        private readonly List<SceneNode> _solidObjects = new List<SceneNode>();
        private readonly List<SceneNode> _transparentObjects = new List<SceneNode>();
        private readonly List<SceneNode> _shadowCastersObjects = new List<SceneNode>();
        private readonly List<PostProcessingStep> _postProcessingSteps = new List<PostProcessingStep>();

        private RenderTarget2D _activeInputSource;
        private RenderTarget2D _activeRenderTarget;
        private readonly RenderTarget2D _directionalLightShadowMap;

        private readonly List<RenderQueue> _renderQueues;
        private readonly RenderQueue _shadowCasterQueue;

        public ForwardSceneRenderer(GameContext context, SceneEffect effect, RenderSettings settings) : base(context)
        {
            _device = context.GraphicsDevice;
            _sceneEffect = effect;
            RenderSettings = settings;

            _directionalLightShadowMap = new RenderTarget2D(_device, RenderSettings.ShadowMapSize, RenderSettings.ShadowMapSize, false, SurfaceFormat.Single, DepthFormat.Depth24);

            var width = context.ScreenBounds.Width;
            var height = context.ScreenBounds.Height;
            _activeInputSource = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            _activeRenderTarget = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

            _shadowCasterQueue = new RenderQueue(context, HandleShadowCasterObjects, GetShadowCasterSceneNodes, _sceneEffect)
            {
                BlendState = BlendState.Opaque,
                DepthStencilState = DepthStencilState.Default,
                RasterizerState = RasterizerState.CullCounterClockwise,
                IsEnabled = true,
                SortNodesBackToFront = false,
            };

            _renderQueues = new List<RenderQueue>
            {
                new RenderQueue(context, HandleSolidObjects, GetSolidObjects, _sceneEffect)
                {
                    DepthStencilState = DepthStencilState.Default,
                    BlendState = BlendState.Opaque,
                    RasterizerState = RasterizerState.CullCounterClockwise,
                },
                new RenderQueue(context, HandleEffectTransparentObjects, GetTransparentObjects, _sceneEffect)
                {
                    DepthStencilState = DepthStencilState.DepthRead,
                    BlendState = BlendState.AlphaBlend,
                    RasterizerState = RasterizerState.CullCounterClockwise,
                    SortNodesBackToFront = true
                }
            };
        }

        private IEnumerable<SceneNode> GetShadowCasterSceneNodes()
        {
            return _shadowCastersObjects;
        }

        public bool EnablePostProcessing { get; set; }

        private IEnumerable<SceneNode> GetTransparentObjects()
        {
            return _transparentObjects;
        }

        private IEnumerable<SceneNode> GetSolidObjects()
        {
            return _solidObjects;
        }
        
        public void AddPostProcessingStep(PostProcessingStep step)
        {
            step.Initialize(_sceneEffect.PostProcessingEffect);
            _postProcessingSteps.Add(step);
        }

        public void Draw(Scene scene)
        {
            RenderStatistics.Instance.StartFrame();
            PreparePostProcessing();

            UpdateNodeLists(scene.AllSceneNodes);

            _sceneEffect.AmbientLight = scene.AmbientLight;

            for (var i = 0; i < scene.AllCameras.Count; i++)
            {
                DrawSceneForCamera(scene, scene.AllCameras[i], scene.HasSceneNodesChanged);
            }

            DoPostProcessing();
            RenderStatistics.Instance.EndFrame();

#if DEBUG_RENDERING
            if (RenderSettings.EnableShadows) DrawDebugShadowMap(GameContext.SpriteBatch, new Rectangle(0,0,128,128));
#endif
            scene.HasSceneNodesChanged = false;
        }

        public RenderSettings RenderSettings { get; }

        private void HandleSolidObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(false, material.IsUnlit, material.ReceiveShadow && RenderSettings.EnableShadows, RenderSettings.EnableSoftShadows);
        }

        private void HandleEffectTransparentObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(false, material.IsUnlit, false, false);
        }

        private void HandleShadowCasterObjects(Material material)
        {
            _sceneEffect.ActivateShadowDepthMapPass();
        }

        private void PreparePostProcessing()
        {
            if (!EnablePostProcessing || !_postProcessingSteps.Any()) return;

            _oldBindings = _device.GetRenderTargets();
            _device.SetRenderTarget(_activeRenderTarget);
        }

        private void DoPostProcessing()
        {
            if (!EnablePostProcessing) return;

            for (var i = 0; i <  _postProcessingSteps.Count; i++)
            {
                var temp = _activeRenderTarget;
                _activeRenderTarget = _activeInputSource;
                _activeInputSource = temp;
                _device.SetRenderTarget(_activeRenderTarget);

                _postProcessingSteps[i].Process(GameContext, _activeInputSource, _activeRenderTarget);
            }

            _device.SetRenderTargets(_oldBindings);
            GameContext.SpriteBatch.Begin();
            GameContext.SpriteBatch.Draw(_activeRenderTarget, Vector2.Zero, Color.White);
            GameContext.SpriteBatch.End();
        }

        private void DrawDebugShadowMap(SpriteBatch spriteBatch, Rectangle target)
        {
            spriteBatch.Begin(effect: _sceneEffect.ShadowMapDebugEffect);
            spriteBatch.Draw(_directionalLightShadowMap, target, Color.White);
            spriteBatch.End();
        }

        private void DrawSceneForCamera(Scene scene, Camera camera, bool hasSceneNodesChanged)
        {
            DrawShadowCastersToDepthmap(scene, camera, hasSceneNodesChanged);
            HandleCameraClearOrSkyPass(camera);

            _sceneEffect.ShadowMap = _directionalLightShadowMap;
            _sceneEffect.ShadowScale = 1.0f/_directionalLightShadowMap.Width;
            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;
            _sceneEffect.LightDirection = scene.Light.Direction;
            _sceneEffect.AmbientIntensity = scene.Light.AmbientIntensity;
            _sceneEffect.DiffuseIntensity = scene.Light.DiffuseIntensity;

            for (var i = 0; i < _renderQueues.Count; i++)
            {
                var renderQueue = _renderQueues[i];
                if (!renderQueue.IsEnabled) continue;
                renderQueue.Draw(camera, scene.Light, hasSceneNodesChanged);
            }
        }

        private void DrawShadowCastersToDepthmap(Scene scene, Camera camera, bool hasSceneNodesChanged)
        {
            if (!RenderSettings.EnableShadows) return;

            scene.Light.UpdateLightViewMatrixForCamera(camera, _shadowCastersObjects);
            _sceneEffect.ShadowMap = null;
            _sceneEffect.LightWorldViewProjection = scene.Light.LightViewMatrix;

            var oldRenderTargets = GameContext.GraphicsDevice.GetRenderTargets();
            GameContext.GraphicsDevice.SetRenderTarget(_directionalLightShadowMap);
            GameContext.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);

            _shadowCasterQueue.Draw(camera, scene.Light, hasSceneNodesChanged);

            GameContext.GraphicsDevice.SetRenderTargets(oldRenderTargets);
        }

        private void HandleCameraClearOrSkyPass(Camera camera)
        {
            var clearFlags = ClearOptions.DepthBuffer;
            if (camera.ClearColor.HasValue) clearFlags |= ClearOptions.Target;

            _device.Clear(clearFlags, camera.ClearColor.GetValueOrDefault(Color.Black), 1.0f, 0);

            var skybox = camera.Skybox;
            if (skybox != null)
            {
                skybox.Update(camera);

                _device.DepthStencilState = DepthStencilState.None;
                _device.BlendState = BlendState.Opaque;

                _sceneEffect.ActivateLightingTechnique(true, true, false, false);
                RenderQueue.DrawElement(camera, skybox.SceneNode, _sceneEffect);

                _device.DepthStencilState = DepthStencilState.Default;
            }
        }

        private void UpdateNodeLists(IList<SceneNode> allNodes)
        {
            _solidObjects.Clear();
            _transparentObjects.Clear();
            _shadowCastersObjects.Clear();

            for (var i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                if (node.Mesh == null || node.Material == null || !node.IsActive) continue;

                if (node.Material.UseTransparency)
                {
                    _transparentObjects.Add(node);
                }
                else
                {
                    _solidObjects.Add(node);
                }

                if (node.Material.CastShadow && !node.Material.UseTransparency)
                {
                    _shadowCastersObjects.Add(node);
                }
            }
        }
    }
}