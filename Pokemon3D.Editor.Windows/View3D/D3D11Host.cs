using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering;
using Pokemon3D.Common;
using Microsoft.Xna.Framework.Content;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Localization;
using System.Windows.Threading;

namespace Pokemon3D.Editor.Windows.View3D
{
    /// <summary>
     /// Host a Direct3D 11 scene.
     /// </summary>
    public class D3D11Host : Image, GameContext
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
        public ShapeRenderer ShapeRenderer { get { throw new NotImplementedException(); } }
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

        /// <summary>
        /// Initializes a new instance of the <see cref="D3D11Host"/> class.
        /// </summary>
        public D3D11Host()
        {
            _timer = new Stopwatch();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
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
        }
        
        private void Unitialize()
        {
        }
        
        private void Render(TimeSpan time)
        {
            _scene.Update((float)time.TotalSeconds);
            _renderer.Draw(_scene);
        }
    }
}
