using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using Pokemon3D.Screens.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens
{
    /// <summary>
    /// A component to manage open screens.
    /// </summary>
    internal class ScreenManager
    {
        private readonly RenderTarget2D _sourceRenderTarget;
        private readonly RenderTarget2D _targetRenderTarget;
        private readonly Dictionary<Type, Screen> _screensByType;
        private readonly Dictionary<Type, ScreenTransition> _screenTransitionsByType;
        private readonly GraphicsDevice _device;

        private bool _executingScreenTransition;
        private bool _quitGame;
        private ScreenTransition _currentTransition;
        
        public Screen CurrentScreen { get; private set; }

        public ScreenManager()
        {
            _device = GameInstance.GetService<GraphicsDevice>();

            var clientBounds = GameInstance.ScreenBounds;
            _sourceRenderTarget = new RenderTarget2D(_device, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _targetRenderTarget = new RenderTarget2D(_device, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _executingScreenTransition = false;
            _currentTransition = new BlendTransition();

            _screensByType = GetImplementationsOf<Screen>().ToDictionary(s => s, s => (Screen)Activator.CreateInstance(s));
            _screenTransitionsByType = GetImplementationsOf<ScreenTransition>().ToDictionary(s => s, s => (ScreenTransition)Activator.CreateInstance(s));
        }
        
        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Type screenType, Type transition = null, object enterInformation = null)
        {
            var oldScreen = CurrentScreen;

            CurrentScreen?.OnClosing();

            CurrentScreen = _screensByType[screenType];
            CurrentScreen.OnOpening(enterInformation);

            if (oldScreen != null && CurrentScreen != null && transition != null)
            {
                PrerenderSourceAndTargetAndMakeTransition(oldScreen, CurrentScreen, transition);
            }
        }

        public void NotifyQuitGame()
        {
            _quitGame = true;
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        public void OnEarlyDraw(GameTime gameTime)
        {
            if (_executingScreenTransition)
            {
                _currentTransition.Draw();
            }
            else
            {
                CurrentScreen?.OnEarlyDraw(gameTime);
            }
        }

        /// <summary>
        /// Draws the current screen.
        /// </summary>
        public void OnLateDraw(GameTime gameTime)
        {
            if (_executingScreenTransition)
            {
                _currentTransition.Draw();
            }
            else
            {
                CurrentScreen?.OnLateDraw(gameTime);
            }
        }

        /// <summary>
        /// Updates the current screen.
        /// </summary>
        public bool Update(GameTime gameTime)
        {
            if (_executingScreenTransition)
            {
                _currentTransition.Update(gameTime);
                if (_currentTransition.IsFinished)
                {
                    _executingScreenTransition = false;
                }
            }
            else
            {
                CurrentScreen?.OnUpdate(gameTime);
            }

            return !_quitGame;
        }

        private void PrerenderSourceAndTargetAndMakeTransition(Screen oldScreen, Screen currentScreen, Type transition)
        {
            _executingScreenTransition = true;
            var currentRenderTarget = _device.GetRenderTargets();

            _device.SetRenderTarget(_sourceRenderTarget);
            oldScreen.OnEarlyDraw(new GameTime());
            oldScreen.OnLateDraw(new GameTime());

            _device.SetRenderTarget(_targetRenderTarget);
            currentScreen.OnEarlyDraw(new GameTime());
            GameInstance.GetService<SceneRenderer>().Draw();
            currentScreen.OnLateDraw(new GameTime());

            _device.SetRenderTargets(currentRenderTarget);
            _currentTransition = _screenTransitionsByType[transition];
            _currentTransition.StartTransition(_sourceRenderTarget, _targetRenderTarget);
        }

        private static IEnumerable<Type> GetImplementationsOf<T>()
        {
            return typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
