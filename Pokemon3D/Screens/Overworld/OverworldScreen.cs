using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Collisions;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Content;
using Pokemon3D.Entities;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering;
using Pokemon3D.Rendering.Compositor;
using Pokemon3D.Screens.MainMenu;
using Pokemon3D.Screens.Transitions;
using static GameProvider;

namespace Pokemon3D.Screens.Overworld
{
    internal class OverworldScreen : Screen, WorldContainer
    {
        public World ActiveWorld { get; private set; }

        private SpriteFont _debugSpriteFont;
        private bool _showRenderStatistics;
        private readonly List<OverworldUIElement> _uiElements = new List<OverworldUIElement>();
        private InputSystem _inputSystem;
        private ScreenManager _screenManager;
        private SceneRenderer _sceneRenderer;
        private CollisionManager _collisionManager;
        private SpriteBatch _spriteBatch;
        private ShapeRenderer _shapeRenderer;
        private bool _isLoaded;

        public void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                ActiveWorld = enterInformation as World;
                if (ActiveWorld == null) throw new InvalidOperationException("Did not receive loaded data.");

                _debugSpriteFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);

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

            lock (_uiElements)
                _uiElements.ForEach(e => { if (e.IsActive) e.Update(gameTime); });

            if (_inputSystem.Keyboard.IsKeyDown(Keys.Escape))
            {
                _screenManager.SetScreen(typeof(MainMenuScreen), typeof(BlendTransition));
            }

            if (_inputSystem.Keyboard.IsKeyDownOnce(Keys.L))
            {
                _sceneRenderer.EnablePostProcessing = !_sceneRenderer.EnablePostProcessing;
            }

            if (_inputSystem.Keyboard.IsKeyDownOnce(Keys.F12))
            {
                _showRenderStatistics = !_showRenderStatistics;
            }

            if (_inputSystem.Keyboard.IsKeyDownOnce(Keys.X) || _inputSystem.GamePad.IsButtonDownOnce(Buttons.X))
            {
                _screenManager.SetScreen(typeof(GameMenu.GameMenuScreen));
            }
        }

        public void OnLateDraw(GameTime gameTime)
        {
            _collisionManager.Draw(ActiveWorld.Player.Camera);
            if (_showRenderStatistics) DrawRenderStatsitics();

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
        }

        private void DrawRenderStatsitics()
        {
            var renderStatistics = RenderStatistics.Instance;
            

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight * 4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(0,GameInstance.ScreenBounds.Height-height);

            _spriteBatch.Begin();
            _shapeRenderer.DrawRectangle((int)startPosition.X,
                (int)startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            _spriteBatch.DrawString(_debugSpriteFont, $"Average DrawTime[ms]: {renderStatistics.AverageDrawTime:0.00}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            _spriteBatch.DrawString(_debugSpriteFont, $"Total Drawcalls: {renderStatistics.DrawCalls}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            _spriteBatch.DrawString(_debugSpriteFont, $"Entity Count: {ActiveWorld.EntitySystem.EntityCount}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            _spriteBatch.DrawString(_debugSpriteFont, $"Mesh Instances: {Rendering.Data.Mesh.InstanceCount}", startPosition, Color.White);
            _spriteBatch.End();
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
