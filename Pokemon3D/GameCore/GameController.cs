using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.GameModes;
using Pokemon3D.UI;
using Pokemon3D.UI.Localization;
using System;
using System.Windows.Threading;
using Pokemon3D.Collisions;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Screens;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Screens.MainMenu;
using System.Threading;
using System.Collections.Generic;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Wraps around the MonoGame <see cref="Game"/> class.
    /// </summary>
    class GameController : Game, GameContext
    {
        private UiOverlay _notificationBarOverlay;
        private Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private Dispatcher _mainThreadDispatcher;
        private Rectangle _currentScreenBounds;
        private GameConfiguration _gameConfig;
        private SceneRenderer _renderer;

        /// <summary>
        /// The singleton instance of the main GameController class.
        /// </summary>
        public static GameController Instance { get; private set; }

        /// <summary>The name of the game.</summary>
        public const string GAME_NAME = "Pokémon3D";
        /// <summary>The current version of the game.</summary>
        public const string VERSION = "1.0";
        /// <summary>The development stage of the game.</summary>
        public const string DEVELOPMENT_STAGE = "Alpha";
        /// <summary>The internal build number of the game. This number will increase with every release.</summary>
        public const string INTERNAL_VERSION = "89";
        /// <summary>If the debug mode is currently active.</summary>
        public const bool IS_DEBUG_ACTIVE = true;
        private SpriteBatch _spriteBatch;
        private InputSystem _inputSystem;
        private ScreenManager _screenManager;
        private CollisionManager _collisionManager;

        public event EventHandler WindowSizeChanged;

        public string VersionInformation => $"{VERSION} {DEVELOPMENT_STAGE}";

        public Rectangle ScreenBounds => _currentScreenBounds;

        public SaveGame LoadedSave { get; set; }

        public GameController()
        {
            if (Instance != null) throw new InvalidOperationException("Game is singleton and can be created just once");

            GameLogger.Instance.Log(MessageType.Message, "Game started.");
            Exiting += OnGameExit;
            Window.ClientSizeChanged += OnClientSizeChanged;

            _currentScreenBounds = Window.ClientBounds;
            Instance = this;

            Content.RootDirectory = "Content";
            _gameConfig = RegisterService(new GameConfiguration());
            RegisterService(new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _gameConfig.WindowSize.Width,
                PreferredBackBufferHeight = _gameConfig.WindowSize.Height
            });
            _mainThreadDispatcher = Dispatcher.CurrentDispatcher;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;

            var renderSettings = new RenderSettings
            {
                EnableShadows = _gameConfig.ShadowsEnabled,
                EnableSoftShadows = _gameConfig.SoftShadows,
                ShadowMapSize = 1024 // todo: reenable
            };

            RegisterService(GraphicsDevice);
            _renderer = RegisterService(SceneRendererFactory.Create(this, new WindowsSceneEffect(Content), renderSettings));
            RegisterService(new GameModeManager());
            _spriteBatch = RegisterService(new SpriteBatch(GraphicsDevice));
            _inputSystem = RegisterService(new InputSystem());
            RegisterService(new ShapeRenderer(_spriteBatch));
            _screenManager = RegisterService(new ScreenManager());
            RegisterService(new CoreTranslationManager());
            _collisionManager = RegisterService(new CollisionManager());

            _notificationBarOverlay = new UiOverlay();
            RegisterService(_notificationBarOverlay.AddElement(new NotificationBar(400)));
            _notificationBarOverlay.Show();

#if DEBUG_RENDERING
            _collisionManager.DrawDebugShapes = true;
#endif
#if DEBUG
            GetService<ScreenManager>().SetScreen(typeof(MainMenuScreen));
#else
            ScreenManager.SetScreen(typeof(IntroScreen));
#endif
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _inputSystem.Update();
            _collisionManager.Update();

            if (!_screenManager.Update(gameTime)) Exit();
            _notificationBarOverlay.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _screenManager.OnEarlyDraw(gameTime);
            _renderer.Draw();
            _screenManager.OnLateDraw(gameTime);
            _notificationBarOverlay.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        private void OnGameExit(object sender, EventArgs e)
        {
            GameLogger.Instance.Log(MessageType.Message, "Exiting game.");
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (WindowSizeChanged != null && _currentScreenBounds != Window.ClientBounds)
                WindowSizeChanged(this, EventArgs.Empty);

            _currentScreenBounds = Window.ClientBounds;
        }

        public void EnsureExecutedInMainThread(Action action)
        {
            if (_mainThreadDispatcher.Thread == Thread.CurrentThread)
            {
                action();
            }
            else
            {
                _mainThreadDispatcher.Invoke(action);
            }
        }

        public TService GetService<TService>() where TService : class
        {
            return _services[typeof(TService)] as TService;
        }

        public TService RegisterService<TService>(TService service)
        {
            if (_services.ContainsKey(typeof(TService))) throw new InvalidOperationException("Service " + typeof(TService).FullName + " already registered");

            _services.Add(typeof(TService), service);
            return service;
        }

        public void ExecuteBackgroundJob(Action action, Action onFinished = null)
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                try
                {
                    action();
                    onFinished?.Invoke();
                }
                catch (Exception ex)
                {
                    GameLogger.Instance.Log(ex);
                }
            });
        }
    }
}
