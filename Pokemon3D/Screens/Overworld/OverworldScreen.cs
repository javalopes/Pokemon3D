using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Collisions;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering;
using static GameProvider;
using Pokemon3D.Rendering.UI;
using Pokemon3D.UI;

namespace Pokemon3D.Screens.Overworld
{
    internal class OverworldScreen : Screen, WorldContainer
    {
        public World ActiveWorld { get; private set; }

        private bool _showRenderStatistics;
        private readonly List<OverworldUIElement> _uiElements = new List<OverworldUIElement>();
        private InputSystem _inputSystem;
        private ScreenManager _screenManager;
        private SceneRenderer _sceneRenderer;
        private CollisionManager _collisionManager;
        private SpriteBatch _spriteBatch;
        private ShapeRenderer _shapeRenderer;
        private bool _isLoaded;

        private UiOverlay _renderStatisticsOverlay;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                ActiveWorld = enterInformation as World;
                if (ActiveWorld == null) throw new InvalidOperationException("Did not receive loaded data.");

                _renderStatisticsOverlay = new UiOverlay();
                _renderStatisticsOverlay.AddElement(new RenderStatisticsView(ActiveWorld));

                GameInstance.LoadedSave = TestMock.CreateTempSave();
                GameInstance.LoadedSave.Load(GameInstance.GetService<GameModeManager>().ActiveGameMode);

                _isLoaded = true;
            }

            _inputSystem = GameInstance.GetService<InputSystem>();
            _screenManager = GameInstance.GetService<ScreenManager>();
            _sceneRenderer = GameInstance.GetService<SceneRenderer>();
            _collisionManager = GameInstance.GetService<CollisionManager>();
            _spriteBatch = GameInstance.GetService<SpriteBatch>();
            _shapeRenderer = GameInstance.GetService<ShapeRenderer>();
        }

        public void OnUpdate(GameTime gameTime)
        {
            ActiveWorld.Update(gameTime);
            _renderStatisticsOverlay.Update(gameTime);

            lock (_uiElements)
                _uiElements.ForEach(e => { if (e.IsActive) e.Update(gameTime); });

            if (_inputSystem.IsPressedOnce(ActionNames.ToggleRenderStatistics))
            {
                _showRenderStatistics = !_showRenderStatistics;
                if (_showRenderStatistics) _renderStatisticsOverlay.Show(); else _renderStatisticsOverlay.Hide();
            }

            //todo: remove when menu works
            if (_inputSystem.KeyboardHandler.IsKeyDownOnce(Keys.X) || _inputSystem.GamePadHandler.IsButtonDownOnce(Buttons.X))
            {
                _screenManager.SetScreen(typeof(GameMenu.GameMenuScreen));
            }
        }

        public void OnLateDraw(GameTime gameTime)
        {
            _collisionManager.Draw(ActiveWorld.Player.Camera);
            _renderStatisticsOverlay.Draw(_spriteBatch);

            bool anyActive;
            lock (_uiElements)
            {
                anyActive = _uiElements.Count > 0 && _uiElements.Any(e => e.IsActive);
            }

            if (anyActive)
            {
                _spriteBatch.Begin();

                lock (_uiElements)
                {
                    _uiElements.ForEach(e => { if (e.IsActive) e.Draw(gameTime); });
                }

                _spriteBatch.End();
            }

#if DEBUG_RENDERING
            _sceneRenderer.LateDebugDraw3D();
#endif
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
            ActiveWorld.OnEarlyDraw();
        }

        public void OnClosing()
        {
        }

        #region overworld ui element handling:
        

        public void AddUiElement(OverworldUIElement element)
        {
            lock (_uiElements) _uiElements.Add(element);
            element.Screen = this;
        }

        public void RemoveUiElement(OverworldUIElement element)
        {
            lock (_uiElements) _uiElements.Remove(element);
        }

        public bool HasBlockingElements
        {
            get
            {
                lock (_uiElements)
                {
                    return _uiElements.Any(e => e.IsActive && e.IsBlocking);
                }
            }
        }

        #endregion
    }
}
