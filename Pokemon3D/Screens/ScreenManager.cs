﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering;
using Pokemon3D.Screens.Transitions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Pokemon3D.GameProvider;

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

        private bool _executingScreenTransition;
        private bool _quitGame;
        private ScreenTransition _currentTransition;
        private readonly List<Screen> _activeOverlays;
        private readonly GraphicsDevice _device;

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
            _activeOverlays = new List<Screen>();
        }

        private static IEnumerable<Type> GetImplementationsOf<T>()
        {
            return typeof(T).Assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract);
        }

        /// <summary>
        /// Sets the current screen to a new screen instance.
        /// </summary>
        public void SetScreen(Type screenType, Type transition = null, object enterInformation = null)
        {
            var oldScreen = CurrentScreen;

            if (CurrentScreen != null)
            {
                CurrentScreen.OnClosing();
                foreach (var overlays in _activeOverlays) overlays.OnClosing();
            }

            _activeOverlays.Clear();
            CurrentScreen = _screensByType[screenType];
            CurrentScreen.OnOpening(enterInformation);

            if (oldScreen != null && CurrentScreen != null && transition != null)
            {
                PrerenderSourceAndTargetAndMakeTransition(oldScreen, CurrentScreen, transition);
            }
        }

        /// <summary>
        /// Adds a screen on top of current screen.
        /// </summary>
        /// <param name="screenType">Screen type to add</param>
        /// <param name="enterInformation">context information</param>
        public void PushScreen(Type screenType, object enterInformation = null)
        {
            var overlay = _screensByType[screenType];
            overlay.OnOpening(enterInformation);
            _activeOverlays.Add(overlay);
        }

        /// <summary>
        /// Removes current topmost popup screen.
        /// </summary>
        public void PopScreen()
        {
            if (_activeOverlays.Count == 0) throw new InvalidOperationException("There is no popup screen to remove.");
            _activeOverlays.Remove(_activeOverlays.Last());
        }

        public void NotifyQuitGame()
        {
            _quitGame = true;
        }

        private void PrerenderSourceAndTargetAndMakeTransition(Screen oldScreen, Screen currentScreen, Type transition)
        {
            _executingScreenTransition = true;
            var currentRenderTarget = _device.GetRenderTargets();

            _device.SetRenderTarget(_sourceRenderTarget);
            oldScreen.OnEarlyDraw(new GameTime());
            oldScreen.OnLateDraw(new GameTime());

            _device.SetRenderTarget(_targetRenderTarget);
            currentScreen.OnUpdate(new GameTime());
            currentScreen.OnEarlyDraw(new GameTime());
            GameInstance.GetService<SceneRenderer>().Draw();
            currentScreen.OnLateDraw(new GameTime());

            _device.SetRenderTargets(currentRenderTarget);
            _currentTransition = _screenTransitionsByType[transition];
            _currentTransition.StartTransition(_sourceRenderTarget, _targetRenderTarget);
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
                foreach (var overlay in _activeOverlays) overlay.OnEarlyDraw(gameTime);
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
                foreach (var overlay in _activeOverlays) overlay.OnLateDraw(gameTime);
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
                if (_activeOverlays.Any())
                {
                    _activeOverlays.Last().OnUpdate(gameTime);
                }
                else
                {
                    CurrentScreen?.OnUpdate(gameTime);
                }
            }

            return !_quitGame;
        }
    }
}
