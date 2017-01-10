using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Pokemon3D.Rendering.Data;

// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering.Compositor
{
    internal class ForwardSceneRenderer : GameContextObject, SceneRenderer
    {
        private readonly object _lockObject = new object();

        private readonly List<Light> _allLights = new List<Light>();
        private readonly List<DrawableElement> _initializingDrawables = new List<DrawableElement>();
        private readonly List<DrawableElement> _allDrawables = new List<DrawableElement>();
        private readonly List<Camera> _allCameras = new List<Camera>();
        private readonly List<DrawableElement> _allShadowCasters = new List<DrawableElement>();
        private readonly List<Action<Camera, SceneRenderer>> _customDrawActions = new List<Action<Camera, SceneRenderer>>();

        private readonly GraphicsDevice _device;
        private readonly EffectProcessor _effectProcessor;
        private readonly SpriteBatch _spriteBatch;
        
        private readonly List<RenderQueue> _renderQueues = new List<RenderQueue>();

        private readonly RenderQueue _shadowCasterQueueSolid;
        private readonly RenderQueue _shadowCasterQueueTransparent;
        private readonly RenderQueue _solidObjectsQueue;
        private readonly RenderQueue _transparentObjectsQueue;

        private readonly RenderTarget2D _cameraOutputRenderTarget;
        
        public ForwardSceneRenderer(GameContext context, EffectProcessor effectProcessor, RenderSettings settings) : base(context)
        {
            _device = context.GetService<GraphicsDevice>();
            _effectProcessor = effectProcessor;
            _spriteBatch = new SpriteBatch(_device);
            RenderSettings = settings;

            _shadowCasterQueueSolid = new ShadowCasterRenderQueue(context, _effectProcessor)
            {
                BlendState = BlendState.Opaque,
                DepthStencilState = DepthStencilState.Default,
                RasterizerState = RasterizerState.CullCounterClockwise,
                IsEnabled = true,
                SortNodesBackToFront = false,
            };
            _shadowCasterQueueTransparent = new ShadowCasterRenderQueue(context, _effectProcessor)
            {
                BlendState = BlendState.AlphaBlend,
                DepthStencilState = DepthStencilState.DepthRead,
                RasterizerState = RasterizerState.CullCounterClockwise,
                IsEnabled = true,
                SortNodesBackToFront = true,
            };

            _solidObjectsQueue = AddRenderQueue(new RenderQueue(context, _effectProcessor)
            {
                DepthStencilState = DepthStencilState.Default,
                BlendState = BlendState.Opaque,
                RasterizerState = RasterizerState.CullCounterClockwise,
            });

            _transparentObjectsQueue = AddRenderQueue(new RenderQueue(context, _effectProcessor)
            {
                DepthStencilState = DepthStencilState.DepthRead,
                BlendState = BlendState.AlphaBlend,
                RasterizerState = RasterizerState.CullCounterClockwise,
                SortNodesBackToFront = true
            });
            
            _cameraOutputRenderTarget = new RenderTarget2D(context.GetService<GraphicsDevice>(),
                                                           context.ScreenBounds.Width, 
                                                           context.ScreenBounds.Height, 
                                                           false, 
                                                           SurfaceFormat.Color, 
                                                           DepthFormat.Depth24);

            DefaultPostProcessors = new DefaultPostProcessors(context, _effectProcessor);
        }

        public RenderSettings RenderSettings { get; }

        public DefaultPostProcessors DefaultPostProcessors { get; }

        public Vector4 AmbientLight { get; set; }

        public void Draw()
        {
            RenderStatistics.Instance.StartFrame();

            lock (_lockObject)
            {
                UpdateNodeLists();
                _effectProcessor.AmbientLight = AmbientLight;

                for (var i = 0; i < _allCameras.Count; i++)
                {
                    var camera = _allCameras[i];
                    if (camera.IsActive) DrawSceneForCamera(camera);
                }
            }

            RenderStatistics.Instance.EndFrame();
        }

        public void RegisterCustomDraw(Action<Camera, SceneRenderer> onDraw)
        {
            _customDrawActions.Add(onDraw);
        }

        public void DrawImmediate(Camera camera, Matrix world, Material material, Mesh mesh)
        {
            _effectProcessor.World = world;
            var passes = _effectProcessor.ApplyByMaterial(material, RenderSettings);
            for (var i = 0; i < passes.Count; i++)
            {
                passes[i].Apply();
                mesh.Draw();
            }
        }

        public DrawableElement CreateDrawableElement(bool initializing, int cameraMask = 1)
        {
            DrawableElement drawableElement;

            lock (_lockObject)
            {
                drawableElement = new DrawableElement(cameraMask, initializing, OnEndInitializing);

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
                    Type = LightType.Directional,
                    ShadowMap = new RenderTarget2D(_device, RenderSettings.ShadowMapSize, RenderSettings.ShadowMapSize, false, SurfaceFormat.Single, DepthFormat.Depth24)
                };
                _allLights.Add(light);
            }
            return light;
        }

        public Camera GetMainCamera()
        {
            return _allCameras.Single(c => c.IsMain);
        }

        public void RemoveDrawableElement(DrawableElement element)
        {
            lock (_lockObject)
            {
                if (!_initializingDrawables.Remove(element)) _allDrawables.Remove(element);
            }
        }

        public Camera CreateCamera(int cameraMask = 1)
        {
            var camera = new Camera(_device.Viewport, cameraMask);
            lock (_lockObject)
            {
                _allCameras.Add(camera);
            }
            return camera;
        }

        public void RemoveCamera(Camera camera)
        {
            lock (_lockObject)
            {
                _allCameras.Remove(camera);
            }
        }
        
        public void LateDebugDraw3D()
        {
            //if (RenderSettings.EnableShadows) DrawDebugShadowMap(GameContext.GetService<SpriteBatch>(), new Rectangle(0, 0, 128, 128));
        }

        public void RemoveLight(Light light)
        {
            lock (_lockObject)
            {
                _allLights.Remove(light);
            }
        }

        public void OnViewSizeChanged(Rectangle oldSize, Rectangle newSize)
        {
            foreach (var camera in _allCameras)
            {
                camera.OnViewSizeChanged(oldSize, newSize);
            }
        }
        
        private RenderQueue AddRenderQueue(RenderQueue renderQueue)
        {
            _renderQueues.Add(renderQueue);
            return renderQueue;
        }
        
        private void DrawSceneForCamera(Camera camera)
        {
            _device.Viewport = camera.Viewport;
            var light = _allLights.FirstOrDefault();

            if (light != null)
            {
                DrawShadowCastersToDepthmap(light, camera);
            }

            RenderTargetBinding[] renderTargetBindings = null;
            if (camera.PostProcess.ShouldProcess)
            {
                renderTargetBindings = _device.GetRenderTargets();
                _device.SetRenderTarget(_cameraOutputRenderTarget);
            }

            HandleCameraClearOrSkyPass(camera);
            
            _effectProcessor.View = camera.ViewMatrix;
            _effectProcessor.Projection = camera.ProjectionMatrix;
            
            if (light != null)
            {
                _effectProcessor.SetLights(_allLights);
            }
            
            for (var i = 0; i < _renderQueues.Count; i++)
            {
                var renderQueue = _renderQueues[i];
                if (!renderQueue.IsEnabled) continue;
                renderQueue.Draw(camera, RenderSettings, camera.GlobalEulerAngles.Y);
            }

            if (camera.PostProcess.ShouldProcess)
            {
                var invScreenSize = new Vector2(1.0f / GameContext.ScreenBounds.Width, 1.0f / GameContext.ScreenBounds.Height);

                var resultTarget = camera.PostProcess.ProcessChain(_spriteBatch, invScreenSize, _cameraOutputRenderTarget);

                _device.SetRenderTargets(renderTargetBindings);
                
                _spriteBatch.Begin();
                _spriteBatch.Draw(resultTarget, GameContext.ScreenBounds, Color.White);
                _spriteBatch.End();
            }

            if (camera.IsMain && _customDrawActions.Count > 0)
            {
                foreach (var action in _customDrawActions)
                {
                    action.Invoke(camera, this);
                }
            }
        }

        private void DrawShadowCastersToDepthmap(Light light, Camera camera)
        {
            if (!RenderSettings.EnableShadows) return;

            //todo: calculating shadow volumne for all visible elements causes performance problems.
            //todo: improve performance and switch back to _allShadowCasters.
            light.UpdateLightViewMatrixForCamera(camera, _solidObjectsQueue.Elements);

            var oldRenderTargets = _device.GetRenderTargets();
            _device.SetRenderTarget(light.ShadowMap);
            _device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            
            var angle = (float)Math.Atan2(light.Direction.Z, light.Direction.X) - MathHelper.Pi/4*3;
            _shadowCasterQueueSolid.Draw(camera, RenderSettings, angle);
            _shadowCasterQueueTransparent.Draw(camera, RenderSettings, angle);

            _device.SetRenderTargets(oldRenderTargets);
        }

        private void HandleCameraClearOrSkyPass(Camera camera)
        {
            var clearColor = camera.ClearColor.HasValue;
            var clearDepth = camera.DepthClear.HasValue;

            if(clearColor && clearDepth)
            {
                _device.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, camera.ClearColor.Value, camera.DepthClear.Value, 0);
            }
            else if(clearColor && !clearDepth)
            {
                _device.Clear(ClearOptions.Target, camera.ClearColor.Value, 1.0f, 0);
            }
            else if(!clearColor && clearDepth)
            {
                _device.Clear(ClearOptions.DepthBuffer, Color.Black, camera.DepthClear.Value, 0);
            }

            var skybox = camera.Skybox;
            if (skybox != null)
            {
                skybox.Update(camera);

                _device.DepthStencilState = DepthStencilState.None;
                _device.BlendState = BlendState.Opaque;

                _effectProcessor.World = skybox.DrawableElement.GetWorldMatrix(camera.GlobalEulerAngles.Y);
                var passes = _effectProcessor.ApplyByMaterial(skybox.DrawableElement.Material, RenderSettings);
                for (var i = 0; i < passes.Count; i++)
                {
                    passes[i].Apply();
                    skybox.DrawableElement.Mesh.Draw();
                }

                _device.DepthStencilState = DepthStencilState.Default;
            }
        }

        private void UpdateNodeLists()
        {
            _solidObjectsQueue.Elements.Clear();
            _transparentObjectsQueue.Elements.Clear();
            _shadowCasterQueueSolid.Elements.Clear();
            _shadowCasterQueueSolid.Elements.Clear();

            for (var i = 0; i < _allDrawables.Count; i++)
            {
                var node = _allDrawables[i];
                if (node.Mesh == null || node.Material == null || !node.IsActive) continue;

                if (node.Material.CastShadow)
                {
                    _allShadowCasters.Add(node);
                    if (node.Material.UseTransparency)
                    {
                        _shadowCasterQueueSolid.Elements.Add(node);
                    }
                    else
                    {
                        _shadowCasterQueueSolid.Elements.Add(node);
                    }
                }

                if (node.Material.UseTransparency)
                {
                    _transparentObjectsQueue.Elements.Add(node);
                }
                else
                {
                    _solidObjectsQueue.Elements.Add(node);
                }
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
    }
}
