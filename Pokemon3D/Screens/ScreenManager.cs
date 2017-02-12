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
        private readonly Dictionary<Type, IScreen> _screensByType;
        private readonly Dictionary<Type, IScreenTransition> _screenTransitionsByType;
        private readonly GraphicsDevice _device;

        private bool _executingScreenTransition;
        private bool _quitGame;
        private IScreenTransition _currentTransition;
        
        public IScreen CurrentIScreen { get; private set; }

        public ScreenManager()
        {
            _device = IGameInstance.GetService<GraphicsDevice>();

            var clientBounds = IGameInstance.ScreenBounds;
            _sourceRenderTarget = new RenderTarget2D(_device, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _targetRenderTarget = new RenderTarget2D(_device, clientBounds.Width, clientBounds.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            _executingScreenTransition = false;
            _currentTransition = new BlendTransition();

            _screensByType = GetImplementationsOf<IScreen>().ToDictionary(s => s, s => (IScreen)Activator.CreateInstance(s));
            _screenTransitionsByType = GetImplementationsOf<IScreenTransition>().ToDictionary(s => s, s => (IScreenTransition)Activator.CreateInstance(s));
        }
        
        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Type screenType, Type transition = null, object enterInformation = null)
        {
            var oldScreen = CurrentIScreen;

            CurrentIScreen?.OnClosing();

            CurrentIScreen = _screensByType[screenType];
            CurrentIScreen.OnOpening(enterInformation);

            if (oldScreen != null && CurrentIScreen != null && transition != null)
            {
                PrerenderSourceAndTargetAndMakeTransition(oldScreen, CurrentIScreen, transition);
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
                CurrentIScreen?.OnEarlyDraw(gameTime);
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
                CurrentIScreen?.OnLateDraw(gameTime);
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
                CurrentIScreen?.OnUpdate(gameTime);
            }

            return !_quitGame;
        }

        private void PrerenderSourceAndTargetAndMakeTransition(IScreen oldIScreen, IScreen currentIScreen, Type transition)
        {
            _executingScreenTransition = true;
            var currentRenderTarget = _device.GetRenderTargets();

            _device.SetRenderTarget(_sourceRenderTarget);
            oldIScreen.OnEarlyDraw(new GameTime());
            oldIScreen.OnLateDraw(new GameTime());

            _device.SetRenderTarget(_targetRenderTarget);
            currentIScreen.OnEarlyDraw(new GameTime());
            IGameInstance.GetService<ISceneRenderer>().Draw();
            currentIScreen.OnLateDraw(new GameTime());

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
