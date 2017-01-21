using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Diagnostics;
using Pokemon3D.GameModes;
using Pokemon3D.UI;
using System;
using Pokemon3D.Collisions;
using Pokemon3D.Screens;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Rendering.UI;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.DataModel.GameCore;
using Pokemon3D.InputSystem;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.ScriptPipeline;

namespace Pokemon3D.GameCore
{
    /// <summary>
    /// Wraps around the MonoGame <see cref="Game"/> class.
    /// </summary>
    internal class GameController : Game, GameContext
    {
        /// <summary>
        /// The singleton instance of the main GameController class.
        /// </summary>
        public static GameController Instance { get; private set; }

        /// <summary>The name of the game.</summary>
        public const string GameName = "Pokémon3D";
        /// <summary>The current version of the game.</summary>
        public const string Version = "1.0";

        /// <summary>The development stage of the game.</summary>
        public const string DevelopmentStage = "Alpha";

        /// <summary>The internal build number of the game. This number will increase with every release.</summary>
        public const string InternalVersion = "89";

        public event EventHandler WindowSizeChanged;
        
        public SaveGame LoadedSave { get; set; }

        public Rectangle ScreenBounds { get; private set; }

        private readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private readonly GameConfiguration _gameConfig;
        private UiOverlay _notificationBarOverlay;
        private SceneRenderer _renderer;
        private SpriteBatch _spriteBatch;
        private InputSystem.InputSystem _inputSystem;
        private ScreenManager _screenManager;
        private CollisionManager _collisionManager;
        private EventAggregator _eventAggregator;

        public GameController()
        {
            if (Instance != null) throw new InvalidOperationException("Game is singleton and can be created just once");

            GameLogger.Instance.Log(MessageType.Message, "Game started.");
            
            Window.ClientSizeChanged += OnClientSizeChanged;

            ScreenBounds = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            Instance = this;

            Content.RootDirectory = "Content";

            _gameConfig = RegisterService(new GameConfiguration());
            RegisterService(new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _gameConfig.Data.WindowSize.Width,
                PreferredBackBufferHeight = _gameConfig.Data.WindowSize.Height
            });

            Exiting += OnGameExit;
        }
        
        protected override void LoadContent()
        {
            base.LoadContent();

            IsMouseVisible = true;
            
            var renderSettings = new RenderSettings
            {
                EnableShadows = _gameConfig.Data.ShadowsEnabled,
                EnableSoftShadows = _gameConfig.Data.SoftShadows,
                ShadowMapSize = _gameConfig.Data.ShadowMapSize
            };

            RegisterService(Window);
            RegisterService(GraphicsDevice);
            RegisterService(Content);
            RegisterService(new JobSystem());
            RegisterService(new ScriptPipelineManager());
            RegisterService(new Random());
            _renderer = RegisterService(SceneRendererFactory.Create(this, new WindowsEffectProcessor(Content), renderSettings));
            RegisterService(new GameModeManager());
            _spriteBatch = RegisterService(new SpriteBatch(GraphicsDevice));
            _inputSystem = RegisterService(new InputSystem.InputSystem());
            RegisterService(new ShapeRenderer(_spriteBatch));
            _screenManager = RegisterService(new ScreenManager());
            RegisterService<TranslationProvider>(new TranslationProviderImp());
            _collisionManager = RegisterService(new CollisionManager());
            _eventAggregator = RegisterService(new EventAggregator());

            _notificationBarOverlay = new UiOverlay();
            RegisterService(_notificationBarOverlay.AddElement(new NotificationBar(400)));
            _notificationBarOverlay.Show();
#if DEBUG_RENDERING
            _collisionManager.DrawDebugShapes = true;
#endif
#if DEBUG
            GetService<ScreenManager>().SetScreen(typeof(MainMenuScreen));
#else
            GetService<ScreenManager>().SetScreen(typeof(IntroScreen));
#endif
            GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            RegisterInputActions();
        }

        private static InputActionModel CreateAxis(string name, InputType type,  string value)
        {
            return new InputActionModel
            {
                Name = name,
                ActionsModel = new[]
                {
                    new MappedActionModel
                    {
                        InputType = type,
                        IsAxis = true,
                        AssingedValue = value
                    }
                }
            };
        }

        private static InputActionModel CreateAction(string name, InputType type, string value)
        {
            return CreateAction(name, type, new[] {value});
        }

        private static InputActionModel CreateAction(string name, InputType type, string[] values)
        {
            return new InputActionModel
            {
                Name = name,
                ActionsModel = values.Select(v =>
                    new MappedActionModel
                    {
                        InputType = type,
                        IsAxis = false,
                        AssingedValue = v
                    }
                ).ToArray()
            };
        }

        private void RegisterInputActions()
        {
            if (_gameConfig.Data.InputActions == null)
            {
                _gameConfig.Data.InputActions = new[]
                {
                    CreateAxis(ActionNames.LeftAxis, InputType.Keyboard, "A,D,W,S"),
                    CreateAxis(ActionNames.LeftAxis, InputType.GamePad, "ThumbStickLeft"),
                    CreateAxis(ActionNames.RightAxis,InputType.Keyboard, "Left,Right,Up,Down"),
                    CreateAxis(ActionNames.RightAxis,InputType.GamePad, "ThumbStickRight"),
                    CreateAction(ActionNames.SprintGodMode, InputType.Keyboard,"LeftShift"),
                    CreateAction(ActionNames.StraveGodMode, InputType.Keyboard,"Space"),
                    CreateAction(ActionNames.MenuUp, InputType.Keyboard,new[] { "Up", "W" }),
                    CreateAction(ActionNames.MenuDown, InputType.Keyboard,new[] { "Down", "S" }),
                    CreateAction(ActionNames.MenuUp, InputType.GamePad, "LeftThumbstickUp"),
                    CreateAction(ActionNames.MenuDown, InputType.GamePad,"LeftThumbstickDown"),
                    CreateAction(ActionNames.MenuAccept, InputType.Keyboard,new[] { "Enter", "Space" }),
                    CreateAction(ActionNames.MenuAccept, InputType.GamePad,"A"),
                    CreateAction(ActionNames.OpenInventory, InputType.Keyboard,"I"),
                    CreateAction(ActionNames.OpenInventory, InputType.GamePad,"B"),
                    CreateAction(ActionNames.ToggleRenderStatistics, InputType.Keyboard,"F12"),
                };
                _gameConfig.Save();
            }

            _inputSystem.LoadFromConfiguration(_gameConfig.Data.InputActions);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _eventAggregator.Update(gameTime);
            _inputSystem.Update(gameTime);
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
            _gameConfig.Save();
            GameLogger.Instance.Log(MessageType.Message, "Exiting game.");
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            var old = ScreenBounds;
            ScreenBounds = new Rectangle(0,0,Window.ClientBounds.Width, Window.ClientBounds.Height);
            if (WindowSizeChanged != null && old != Window.ClientBounds)
            {
                WindowSizeChanged(this, EventArgs.Empty);
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
    }
}
