using System;
using Microsoft.Xna.Framework;
using Pokemon3D.Entities;
using Pokemon3D.GameCore;
using Pokemon3D.GameModes;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Screens.Transitions;
using Pokemon3D.UI;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.Screens
{
    internal class OverworldScreen : ScreenWithOverlays, WorldContainer
    {
        public World ActiveWorld { get; private set; }

        private bool _showRenderStatistics;
        private InputSystem.InputSystem _inputSystem;
        private ScreenManager _screenManager;
        private bool _isLoaded;

        private UiOverlay _renderStatisticsOverlay;

        public override void OnOpening(object enterInformation)
        {
            if (!_isLoaded)
            {
                ActiveWorld = enterInformation as World;
                if (ActiveWorld == null) throw new InvalidOperationException("Did not receive loaded data.");

                _renderStatisticsOverlay = AddOverlay(new UiOverlay());
                _renderStatisticsOverlay.AddElement(new RenderStatisticsView(ActiveWorld));
                
                GameInstance.GetService<GameModeManager>().ActiveGameMode.LoadSaveGame(TestMock.CreateTempSave());

                _isLoaded = true;
            }

            _inputSystem = GameInstance.GetService<InputSystem.InputSystem>();
            _screenManager = GameInstance.GetService<ScreenManager>();

            GameInstance.GetService<EventAggregator>().Subscribe<GameEvent>(OnGameEventRaised);
        }

        private void OnGameEventRaised(GameEvent gameEvent)
        {
            if (gameEvent.Category == GameEvent.GameQuitToMainMenu)
            {
                ActiveWorld.Clear();
                ActiveWorld = null;
                _isLoaded = false;

                _screenManager.SetScreen(typeof(MainMenuScreen), typeof(SlideTransition));
                GameInstance.GetService<GameModeManager>().UnloadGameMode();
            }
        }

        public override void OnUpdate(GameTime gameTime)
        {
            base.OnUpdate(gameTime);
            ActiveWorld?.Update(gameTime);
            _renderStatisticsOverlay.Update(gameTime);

            if (_inputSystem.IsPressedOnce(ActionNames.ToggleRenderStatistics))
            {
                _showRenderStatistics = !_showRenderStatistics;
                if (_showRenderStatistics) _renderStatisticsOverlay.Show(); else _renderStatisticsOverlay.Hide();
            }
        }

        public override void OnEarlyDraw(GameTime gameTime)
        {
            ActiveWorld?.OnEarlyDraw();
        }

        public override void OnClosing()
        {
            GameInstance.GetService<EventAggregator>().Unsubscribe<GameEvent>(OnGameEventRaised);
        }
    }
}
