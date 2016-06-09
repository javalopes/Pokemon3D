using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering.Compositor
{
    class ForwardSceneRenderer : GameContextObject, SceneRenderer
    {
        private readonly object _lockObject = new object();

        private readonly List<Light> _allLights; 
        private readonly List<DrawableElement> _initializingDrawables;
        private readonly List<DrawableElement> _allDrawables;
        private readonly List<Camera> _allCameras;

        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private RenderTargetBinding[] _oldBindings;

        private readonly List<DrawableElement> _solidObjects = new List<DrawableElement>();
        private readonly List<DrawableElement> _transparentObjects = new List<DrawableElement>();
        private readonly List<DrawableElement> _shadowCastersObjectsSolid = new List<DrawableElement>();
        private readonly List<DrawableElement> _shadowCastersObjectsTransparent = new List<DrawableElement>();
        private readonly List<PostProcessingStep> _postProcessingSteps = new List<PostProcessingStep>();

        private RenderTarget2D _activeInputSource;
        private RenderTarget2D _activeRenderTarget;
        private readonly RenderTarget2D _directionalLightShadowMap;

        private readonly List<RenderQueue> _renderQueues;
        private readonly RenderQueue _shadowCasterQueueSolid;
        private readonly RenderQueue _shadowCasterQueueTransparent;

        public ForwardSceneRenderer(GameContext context, SceneEffect effect, RenderSettings settings) : base(context)
        {
            _device = context.GraphicsDevice;
            _sceneEffect = effect;
            RenderSettings = settings;
            _allDrawables = new List<DrawableElement>();
            _initializingDrawables = new List<DrawableElement>();
            _allCameras = new List<Camera>();
            _allLights = new List<Light>();

            _directionalLightShadowMap = new RenderTarget2D(_device, RenderSettings.ShadowMapSize, RenderSettings.ShadowMapSize, false, SurfaceFormat.Single, DepthFormat.Depth24);

            var width = context.ScreenBounds.Width;
            var height = context.ScreenBounds.Height;
            _activeInputSource = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
            _activeRenderTarget = new RenderTarget2D(_device, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);

            _shadowCasterQueueSolid = new RenderQueue(context, HandleShadowCasterObjects, GetShadowCasterSceneNodes, _sceneEffect)
            {
                BlendState = BlendState.Opaque,
                DepthStencilState = DepthStencilState.Default,
                RasterizerState = RasterizerState.CullCounterClockwise,
                IsEnabled = true,
                SortNodesBackToFront = false,
            };
            _shadowCasterQueueTransparent = new RenderQueue(context, HandleShadowCasterObjects, GetShadowCasterSceneNodesTransparent, _sceneEffect)
            {
                BlendState = BlendState.AlphaBlend,
                DepthStencilState = DepthStencilState.DepthRead,
                RasterizerState = RasterizerState.CullCounterClockwise,
                IsEnabled = true,
                SortNodesBackToFront = true,
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

        private IList<DrawableElement> GetShadowCasterSceneNodesTransparent()
        {
            return _shadowCastersObjectsTransparent;
        }

        private IList<DrawableElement> GetShadowCasterSceneNodes()
        {
            return _shadowCastersObjectsSolid;
        }

        public bool EnablePostProcessing { get; set; }

        private IList<DrawableElement> GetTransparentObjects()
        {
            return _transparentObjects;
        }

        private IList<DrawableElement> GetSolidObjects()
        {
            return _solidObjects;
        }
        
        public void AddPostProcessingStep(PostProcessingStep step)
        {
            step.Initialize(_sceneEffect.PostProcessingEffect);
            _postProcessingSteps.Add(step);
        }

        public void Draw()
        {
            RenderStatistics.Instance.StartFrame();
            PreparePostProcessing();

            lock (_lockObject)
            {
                UpdateNodeLists();
                _sceneEffect.AmbientLight = AmbientLight;

                for (var i = 0; i < _allCameras.Count; i++)
                {
                    if (_allCameras[i].IsActive) DrawSceneForCamera(_allCameras[i]);
                }
            }

            DoPostProcessing();
            RenderStatistics.Instance.EndFrame();
        }
        
        public RenderSettings RenderSettings { get; }

        private void HandleSolidObjects(Data.Material material)
        {
            var flags = material.GetLightingTypeFlags(RenderSettings);
            _sceneEffect.ActivateLightingTechnique(flags);
        }

        private void HandleEffectTransparentObjects(Data.Material material)
        {
            _sceneEffect.ActivateLightingTechnique(LightTechniqueFlag.UseTexture);
        }

        private void HandleShadowCasterObjects(Data.Material material)
        {
            _sceneEffect.ActivateShadowDepthMapPass(material.UseTransparency);
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

        private void DrawSceneForCamera(Camera camera)
        {
            _device.Viewport = camera.Viewport;

            var light = _allLights.FirstOrDefault();

            if (light != null) DrawShadowCastersToDepthmap(light, camera);
            HandleCameraClearOrSkyPass(camera);

            _sceneEffect.ShadowMap = _directionalLightShadowMap;
            _sceneEffect.ShadowScale = 1.0f/_directionalLightShadowMap.Width;
            _sceneEffect.View = camera.ViewMatrix;
            _sceneEffect.Projection = camera.ProjectionMatrix;

            if (light != null)
            {
                _sceneEffect.LightDirection = light.Direction;
                _sceneEffect.AmbientIntensity = light.AmbientIntensity;
                _sceneEffect.DiffuseIntensity = light.DiffuseIntensity;
            }
            
            for (var i = 0; i < _renderQueues.Count; i++)
            {
                var renderQueue = _renderQueues[i];
                if (!renderQueue.IsEnabled) continue;
                renderQueue.Draw(camera, camera.GlobalEulerAngles.Y);
            }
        }

        private void DrawShadowCastersToDepthmap(Light light, Camera camera)
        {
            if (!RenderSettings.EnableShadows) return;

            light.UpdateLightViewMatrixForCamera(camera, _shadowCastersObjectsSolid);
            _sceneEffect.ShadowMap = null;
            _sceneEffect.LightViewProjection = light.LightViewMatrix;

            var oldRenderTargets = GameContext.GraphicsDevice.GetRenderTargets();
            GameContext.GraphicsDevice.SetRenderTarget(_directionalLightShadowMap);
            GameContext.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            
            var angle = (float)Math.Atan2(light.Direction.Z, light.Direction.X) - MathHelper.Pi/4*3;
            _shadowCasterQueueSolid.Draw(camera, angle);
           _shadowCasterQueueTransparent.Draw(camera, angle);

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

                _sceneEffect.ActivateLightingTechnique(LightTechniqueFlag.UseTexture | LightTechniqueFlag.LinearTextureSampling);
                RenderQueue.DrawElement(camera, skybox.DrawableElement, _sceneEffect, 0.0f);

                _device.DepthStencilState = DepthStencilState.Default;
            }
        }

        private void UpdateNodeLists()
        {
            _solidObjects.Clear();
            _transparentObjects.Clear();
            _shadowCastersObjectsSolid.Clear();
            _shadowCastersObjectsTransparent.Clear();

            for (var i = 0; i < _allDrawables.Count; i++)
            {
                var node = _allDrawables[i];
                if (node.Mesh == null || node.Material == null || !node.IsActive) continue;

                if (node.Material.CastShadow)
                {
                    if (node.Material.UseTransparency)
                    {
                        _shadowCastersObjectsTransparent.Add(node);
                    }
                    else
                    {
                        _shadowCastersObjectsSolid.Add(node);
                    }
                }

                if (node.Material.UseTransparency)
                {
                    _transparentObjects.Add(node);
                }
                else
                {
                    _solidObjects.Add(node);
                }
            }
        }

        public DrawableElement CreateDrawableElement(bool initializing)
        {
            DrawableElement drawableElement;

            lock (_lockObject)
            {
                drawableElement = new DrawableElement(initializing, OnEndInitializing);

                if (initializing)
                {
                    _initializingDrawables.Add(drawableElement);
                }
                else
                {
                    _allDrawables.Add(drawableElement);
                }
            }

            return drawableElement;
        }

        public Light CreateDirectionalLight(Vector3 direction)
        {
            Light light;
            lock (_lockObject)
            {
                light = new Light
                {
                    Direction = direction,
                    Type = LightType.Directional
                };
                _allLights.Add(light);
            }
            return light;
        }

        public void RemoveDrawableElement(DrawableElement element)
        {
            lock (_lockObject)
            {
                if (!_initializingDrawables.Remove(element)) _allDrawables.Remove(element);
            }
        }

        private void OnEndInitializing(DrawableElement element)
        {
            lock (_lockObject)
            {
                if (_initializingDrawables.Remove(element))
                {
                    _allDrawables.Add(element);
                }
            }
        }

        public Camera CreateCamera()
        {
            var camera = new Camera(GameContext.GraphicsDevice.Viewport);
            lock (_lockObject)
            {
                _allCameras.Add(camera);
            }
            return camera;
        }

        /// <summary>
        /// Ambient Light for all Objects. Default is white.
        /// </summary>
        public Vector4 AmbientLight { get; set; }

        public void LateDebugDraw3D()
        {
            if (RenderSettings.EnableShadows) DrawDebugShadowMap(GameContext.SpriteBatch, new Rectangle(0, 0, 128, 128));
        }
        
        public void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize)
        {
            foreach (var camera in _allCameras)
            {
                camera.OnViewSizeChanged(oldSize, newSize);
            }
        }
    }
}
