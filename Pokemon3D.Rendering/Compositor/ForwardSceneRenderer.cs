using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Rendering.Data;
using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable ForCanBeConvertedToForeach

namespace Pokemon3D.Rendering.Compositor
{
    class ForwardSceneRenderer : GameContextObject, SceneRenderer
    {
        private readonly GraphicsDevice _device;
        private readonly SceneEffect _sceneEffect;
        private RenderTargetBinding[] _oldBindings;

        private readonly HashSet<int> _registeredStaticNodes = new HashSet<int>();
        private readonly List<StaticMeshBatch> _staticBatches = new List<StaticMeshBatch>();

        private readonly List<DrawableElement> _allDrawableElements = new List<DrawableElement>(); 
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

        public void Draw(Scene scene)
        {
            RenderStatistics.Instance.StartFrame();
            PreparePostProcessing();

            lock (scene.LockObject)
            {
                UpdateStaticNodes(scene.StaticNodes);
                UpdateNodeLists(scene.AllSceneNodes);

                _sceneEffect.AmbientLight = scene.AmbientLight;

                for (var i = 0; i < scene.AllCameras.Count; i++)
                {
                    DrawSceneForCamera(scene, scene.AllCameras[i], scene.HasSceneNodesChanged);
                }
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
            var flags = material.GetLightingTypeFlags(RenderSettings);
            _sceneEffect.ActivateLightingTechnique(flags);
        }

        private void HandleEffectTransparentObjects(Material material)
        {
            _sceneEffect.ActivateLightingTechnique(LightTechniqueFlag.UseTexture);
        }

        private void HandleShadowCasterObjects(Material material)
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

        private void DrawSceneForCamera(Scene scene, Camera camera, bool hasSceneNodesChanged)
        {
            _device.Viewport = camera.Viewport;

            DrawShadowCastersToDepthmap(scene, camera);
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
                renderQueue.Draw(camera, scene.Light, camera.GlobalEulerAngles.Y);
            }
        }

        private void DrawShadowCastersToDepthmap(Scene scene, Camera camera)
        {
            if (!RenderSettings.EnableShadows) return;

            scene.Light.UpdateLightViewMatrixForCamera(camera, _shadowCastersObjectsSolid);
            _sceneEffect.ShadowMap = null;
            _sceneEffect.LightViewProjection = scene.Light.LightViewMatrix;

            var oldRenderTargets = GameContext.GraphicsDevice.GetRenderTargets();
            GameContext.GraphicsDevice.SetRenderTarget(_directionalLightShadowMap);
            GameContext.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            
            var angle = (float)Math.Atan2(scene.Light.Direction.Z, scene.Light.Direction.X) - MathHelper.Pi/4*3;
            _shadowCasterQueueSolid.Draw(camera, scene.Light, angle);
           _shadowCasterQueueTransparent.Draw(camera, scene.Light, angle);

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
                RenderQueue.DrawElement(camera, skybox.SceneNode, _sceneEffect, 0.0f);

                _device.DepthStencilState = DepthStencilState.Default;
            }
        }

        private readonly List<StaticMeshBatch> _batchesToUpdate = new List<StaticMeshBatch>(); 

        private void UpdateStaticNodes(List<SceneNode> staticNodes)
        {
            var sortedNodes = staticNodes.OrderBy(n => n.Material.CompareId).ToArray();

            _batchesToUpdate.Clear();
            for (var i = 0; i < sortedNodes.Length; i++)
            {
                var currentNode = staticNodes[i];
                if (_registeredStaticNodes.Contains(currentNode.Id)) continue;

                var materialId = currentNode.Material.CompareId;

                _registeredStaticNodes.Add(currentNode.Id);

                var addedToExisting = false;
                foreach (var suitableBatch in _staticBatches.Where(s => s.Material.CompareId == materialId))
                {
                    addedToExisting |= suitableBatch.AddBatch(currentNode);
                    if (addedToExisting)
                    {
                        _batchesToUpdate.Add(suitableBatch);
                        break;
                    }
                }

                if (!addedToExisting)
                {
                    var staticBatch = new StaticMeshBatch(GameContext, currentNode.Material);
                    staticBatch.AddBatch(currentNode);
                    _staticBatches.Add(staticBatch);
                    _batchesToUpdate.Add(staticBatch);
                }
            }

            for (int i = 0; i < _batchesToUpdate.Count; i++)
            {
                _batchesToUpdate[i].Build();
            }
        }

        private void UpdateNodeLists(IList<SceneNode> allDynamicNodes)
        {
            _allDrawableElements.Clear();
            _allDrawableElements.AddRange(_staticBatches);
            _allDrawableElements.AddRange(allDynamicNodes);

            _solidObjects.Clear();
            _transparentObjects.Clear();
            _shadowCastersObjectsSolid.Clear();
            _shadowCastersObjectsTransparent.Clear();

            for (var i = 0; i < _allDrawableElements.Count; i++)
            {
                var node = _allDrawableElements[i];
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
    }
}
