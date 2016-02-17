using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering;
using Pokemon3D.Common;
using Microsoft.Xna.Framework.Content;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Localization;
using System.Windows.Threading;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Rendering.Data;
using System.IO;
using System.Collections.Generic;

namespace Pokemon3D.Editor.Windows.View3D
{
    /// <summary>
    /// Host a Direct3D 11 scene.
    /// </summary>
    public class D3D11Host : Image, GameContext, ModelMeshContext
    {
        private static GraphicsDevice _graphicsDevice;
        private static int _referenceCount;
        private static readonly object _graphicsDeviceLock = new object();
        private static bool? _isInDesignMode;

        private RenderTarget2D _renderTarget;
        private D3D11Image _d3D11Image;
        private bool _resetBackBuffer;
        private readonly Stopwatch _timer;
        private TimeSpan _lastRenderingTime;

        private WpfSceneEffect _sceneEffect;
        private SceneRenderer _renderer;
        private Scene _scene;
        private List<SceneNode> _customNodes = new List<SceneNode>();

        /// <summary>
        /// Gets a value indicating whether the controls runs in the context of a designer (e.g.
        /// Visual Studio Designer or Expression Blend).
        /// </summary>
        /// <value>
        /// <see langword="true" /> if controls run in design mode; otherwise, 
        /// <see langword="false" />.
        /// </value>
        public static bool IsInDesignMode
        {
            get
            {
                if (!_isInDesignMode.HasValue)
                {
                    _isInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement)).Metadata.DefaultValue;
                }
                return _isInDesignMode.Value;
            }
        }

        private int ScreenWidth { get { return Math.Max((int)ActualWidth, 1); } }
        private int ScreenHeight { get { return Math.Max((int)ActualHeight, 1); } }

        /// <summary>
        /// Gets the graphics device.
        /// </summary>
        /// <value>The graphics device.</value>
        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        public InputSystem InputSystem { get { throw new NotImplementedException(); } }
        public ContentManager Content { get { throw new NotImplementedException(); } }
        public SpriteBatch SpriteBatch { get { throw new NotImplementedException(); } }
        public TranslationProvider TranslationProvider { get { throw new NotImplementedException(); } }
        public string VersionInformation { get { throw new NotImplementedException(); } }

        public Rectangle ScreenBounds
        {
            get
            {
                return new Rectangle(0, 0, ScreenWidth, ScreenHeight);
            }
        }
        
        public Dispatcher MainThreadDispatcher
        {
            get { return Application.Current.Dispatcher; }
        }

        ShapeRenderer GameContext.ShapeRenderer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="D3D11Host"/> class.
        /// </summary>
        public D3D11Host()
        {
            _timer = new Stopwatch();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            MouseMove += D3D11Host_MouseMove;
            SizeChanged += D3D11Host_SizeChanged;
            PreviewMouseWheel += D3D11Host_PreviewMouseWheel;
        }

        private void D3D11Host_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var position = _camera.Position;
            if (e.Delta > 0)
            {
                position.Z = Math.Min(50, position.Z + 0.4f);
            }
            else if (e.Delta < 0)
            {
                position.Z = Math.Max(5, position.Z - 0.4f);
            }
            _camera.Position = position;
        }

        private void D3D11Host_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _scene?.OnViewSizeChanged(new Rectangle(0,0,(int)e.PreviousSize.Width, (int)e.PreviousSize.Height),
                                      new Rectangle(0, 0, (int)e.NewSize.Width, (int)e.NewSize.Height));
            CreateBackBuffer();
        }

        private bool _isFirstMouseMove = true;
        private System.Windows.Point _lastMousePosition;

        private void D3D11Host_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var currentPosition = e.GetPosition(this);

            if (!_isFirstMouseMove)
            {
                var dx = (float)(_lastMousePosition.X - currentPosition.X) / ScreenWidth;
                var dy = (float)( _lastMousePosition.Y - currentPosition.Y) / ScreenHeight;

                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    _cameraHolder.RotateY(dx * MathHelper.TwoPi);
                    _cameraHolder.RotateX(dy * MathHelper.TwoPi);
                }
                else if (e.RightButton == System.Windows.Input.MouseButtonState.Pressed)
                {
                    var forwardFlat = Vector3.Normalize(new Vector3(_cameraHolder.Forward.X, 0.0f, _cameraHolder.Forward.Z));
                    var rightFlat = Vector3.Normalize(new Vector3(_cameraHolder.Right.X, 0.0f, _cameraHolder.Right.Z));

                    _cameraHolder.Position += forwardFlat * -dy * 4.0f + rightFlat * dx * 4.0f;
                }
            }
            
            _lastMousePosition = currentPosition;
            _isFirstMouseMove = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs eventArgs)
        {
            if (IsInDesignMode)
                return;

            InitializeGraphicsDevice();
            InitializeImageSource();
            Initialize();
            StartRendering();
        }

        private void OnUnloaded(object sender, RoutedEventArgs eventArgs)
        {
            if (IsInDesignMode) return;

            StopRendering();
            Unitialize();
            UnitializeImageSource();
            UninitializeGraphicsDevice();
        }

        private static void InitializeGraphicsDevice()
        {
            lock (_graphicsDeviceLock)
            {
                _referenceCount++;
                if (_referenceCount == 1)
                {
                    // Create Direct3D 11 device.
                    var presentationParameters = new PresentationParameters
                    {
                        // Do not associate graphics device with window.
                        DeviceWindowHandle = IntPtr.Zero,
                    };
                    _graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, presentationParameters);
                }
            }
        }

        private static void UninitializeGraphicsDevice()
        {
            lock (_graphicsDeviceLock)
            {
                _referenceCount--;
                if (_referenceCount == 0)
                {
                    _graphicsDevice.Dispose();
                    _graphicsDevice = null;
                }
            }
        }

        private void InitializeImageSource()
        {
            _d3D11Image = new D3D11Image();
            _d3D11Image.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;
            CreateBackBuffer();
            Source = _d3D11Image;
        }


        private void UnitializeImageSource()
        {
            _d3D11Image.IsFrontBufferAvailableChanged -= OnIsFrontBufferAvailableChanged;
            Source = null;

            if (_d3D11Image != null)
            {
                _d3D11Image.Dispose();
                _d3D11Image = null;
            }
            if (_renderTarget != null)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }
        }
        
        private void CreateBackBuffer()
        {
            _d3D11Image.SetBackBuffer(null);
            if (_renderTarget != null)
            {
                _renderTarget.Dispose();
                _renderTarget = null;
            }

            _renderTarget = new RenderTarget2D(_graphicsDevice, 
                                               ScreenWidth, 
                                               ScreenHeight, 
                                               false, 
                                               SurfaceFormat.Bgr32, 
                                               DepthFormat.Depth24Stencil8, 
                                               0, 
                                               RenderTargetUsage.DiscardContents, 
                                               true);

            _d3D11Image.SetBackBuffer(_renderTarget);
        }

        private void StartRendering()
        {
            if (_timer.IsRunning) return;

            CompositionTarget.Rendering += OnRendering;
            _timer.Start();
        }

        private void StopRendering()
        {
            if (!_timer.IsRunning) return;

            CompositionTarget.Rendering -= OnRendering;
            _timer.Stop();
        }

        private void OnRendering(object sender, EventArgs eventArgs)
        {
            if (!_timer.IsRunning) return;

            // Recreate back buffer if necessary.
            if (_resetBackBuffer) CreateBackBuffer();

            // CompositionTarget.Rendering event may be raised multiple times per frame
            // (e.g. during window resizing).
            var renderingEventArgs = (RenderingEventArgs)eventArgs;
            if (_lastRenderingTime != renderingEventArgs.RenderingTime || _resetBackBuffer)
            {
                _lastRenderingTime = renderingEventArgs.RenderingTime;

                GraphicsDevice.SetRenderTarget(_renderTarget);
                Render(_timer.Elapsed);
                GraphicsDevice.Flush();
            }

            // Always invalidate D3DImage to reduce flickering during window resizing.
            _d3D11Image.Invalidate();

            _resetBackBuffer = false;
        }

        /// <summary>
        /// Raises the <see cref="FrameworkElement.SizeChanged" /> event, using the specified 
        /// information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            _resetBackBuffer = true;
            base.OnRenderSizeChanged(sizeInfo);
        }

        private void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs eventArgs)
        {
            if (_d3D11Image.IsFrontBufferAvailable)
            {
                StartRendering();
                _resetBackBuffer = true;
            }
            else
            {
                StopRendering();
            }
        }

        private SceneNode _cameraHolder;
        private Camera _camera;
        
        private void Initialize()
        {
            _sceneEffect = new WpfSceneEffect(GraphicsDevice, "EditorContent");
            _renderer = SceneRendererFactory.Create(this, _sceneEffect, new RenderSettings
            {
                EnableShadows = false,
                EnableSoftShadows = false,
                ShadowMapSize = 32
            });
            
            _scene = new Scene(this);
            _scene.Light.Direction = new Vector3(1, -1, -1);
             _scene.Light.AmbientIntensity = 0.5f;
             _scene.Light.DiffuseIntensity = 0.5f;

            _cameraHolder = _scene.CreateSceneNode();

            _camera = _scene.CreateCamera();
            _camera.Position = new Vector3(0, 0, 20);
            _camera.SetParent(_cameraHolder);

            _cameraHolder.RotateX(MathHelper.ToRadians(-45));

            var sceneNode = _scene.CreateSceneNode();
            sceneNode.Mesh = new Mesh(GraphicsDevice, CreateGroundFloorGeometryData(10, 1.0f), PrimitiveType.LineList);
            sceneNode.Material = new Material
            {
                CastShadow = false,
                ReceiveShadow = false,
                IsUnlit = true,
                Color = Color.White
            };
        }

        public void Activate3DModel(string filePath)
        {
            foreach(var model in Rendering.Data.ModelMesh.LoadFromFile(this, filePath))
            {
                var node = _scene.CreateSceneNode(true);
                node.Mesh = model.Mesh;
                node.Material = model.Material;
                node.EndInitializing();
                _customNodes.Add(node);
            }
        }

        public void Deactivate3D()
        {
            foreach(var node in _customNodes)
            {
                _scene.RemoveSceneNode(node);
            }
            _lastExceptionMessage = null;
        }

        private GeometryData CreateGroundFloorGeometryData(int cells, float cellSize)
        {
            var geometryData = new GeometryData
            {
                Vertices = new VertexPositionNormalTexture[(cells + 1) * (cells + 1)],
                Indices = new ushort[(cells * (cells + 1) + cells * (cells + 1)) * 2]
            };

            for (var x = 0; x <= cells; x++)
            {
                for (var z = 0; z <= cells; z++)
                {
                    var position = new Vector3(
                        -cells * cellSize * 0.5f + x * cellSize,
                        0.0f,
                        -cells * cellSize * 0.5f + z * cellSize);

                    geometryData.Vertices[z * (cells + 1) + x] = new VertexPositionNormalTexture(position, Vector3.Up, Vector2.Zero);
                }
            }

            var baseVertexIndex = 0;
            var baseIndex = 0;
            for (var z = 0; z <= cells; z++)
            {
                for (var x = 0; x < cells; x++)
                {
                    geometryData.Indices[baseIndex + 0] = (ushort)(baseVertexIndex);
                    geometryData.Indices[baseIndex + 1] = (ushort)(baseVertexIndex + 1);
                    baseVertexIndex += 1;
                    baseIndex += 2;
                }
                baseVertexIndex += 1;
            }

            baseVertexIndex = 0;
            for (var z = 0; z < cells; z++)
            {
                for (var x = 0; x <= cells; x++)
                {
                    geometryData.Indices[baseIndex + 0] = (ushort)(baseVertexIndex);
                    geometryData.Indices[baseIndex + 1] = (ushort)(baseVertexIndex + cells + 1);
                    baseVertexIndex += 1;
                    baseIndex += 2;
                }
            }

            return geometryData;
        }
        
        private void Unitialize()
        {
        }

        private string _lastExceptionMessage;
        
        private void Render(TimeSpan time)
        {
            try
            {
                _scene.Update((float)time.TotalSeconds);
                _renderer.Draw(_scene);
            }
            catch(Exception ex)
            {
                if (ex.Message != _lastExceptionMessage)
                {
                    _lastExceptionMessage = ex.Message;
                    OnErrorOccurred?.Invoke(_lastExceptionMessage);
                }
            }
        }

        public Action<string> OnErrorOccurred;

        public Texture2D GetTextureFromRawFolder(string path)
        {
            using (var memoryStream = new MemoryStream(File.ReadAllBytes(path)))
            {
                memoryStream.Seek(0, SeekOrigin.Begin);
                return Texture2D.FromStream(GraphicsDevice, memoryStream);
            }
        }
    }
}
