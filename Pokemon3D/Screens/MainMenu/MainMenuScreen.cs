using Microsoft.Xna.Framework;
using Pokemon3D.Screens.Transitions;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Screens.GameModeLoading;
using Pokemon3D.UI;
using Pokemon3D.UI.Dialogs;
using static GameProvider;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Localization;

namespace Pokemon3D.Screens.MainMenu
{
    internal class MainMenuScreen : Screen
    {
        private UiOverlay _mainOverlay;
        private SelectionDialog _closeDialog;

        public void OnOpening(object enterInformation)
        {
            var graphicsDeviceManager = GameInstance.GetService<GraphicsDeviceManager>();
            graphicsDeviceManager.PreferMultiSampling = true;
            graphicsDeviceManager.ApplyChanges();

            _mainOverlay = new UiOverlay();

            var translation = GameInstance.GetService<TranslationProvider>();

            _mainOverlay.AddElement(new HexagonBackground());
            _mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "StartNewGame"), new Vector2(26, 45), OnStartNewGame));
            _mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "LoadGame"), new Vector2(26, 107), OnLoadGame));
            _mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "Gamejolt"), new Vector2(26, 169), null));
            _mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "Options"), new Vector2(26, 231), null));
            _mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "QuitGame"), new Vector2(26, 293), OnExitGame));

            _mainOverlay.AddInputController(new KeyboardUiInputController());
            _mainOverlay.AutoEnumerateTabIndices();

            _closeDialog = new SelectionDialog(translation.CreateValue("System", "ReallyQuitGameQuestion"), translation.CreateValue("System", "UnsavedChangesLost"), new[]
            {
                new LeftSideButton(translation.CreateValue("System", "No"), new Vector2(50, 50), b =>
                {
                    _mainOverlay.CloseModal();
                }),
                new LeftSideButton(translation.CreateValue("System", "Yes"), new Vector2(50, 100), b =>
                {
                    GameInstance.GetService<ScreenManager>().NotifyQuitGame();
                })
            });

            var bar = new ControlBar();
            bar.AddEntry(translation.CreateValue("System", "Select"), Buttons.A, Keys.Enter);
            _mainOverlay.AddElement(bar);
            _mainOverlay.Show();
        }

        private void OnExitGame(LeftSideButton button)
        {
            _mainOverlay.ShowModal(_closeDialog);
        }

        private void OnStartNewGame(LeftSideButton button)
        {
            GameInstance.GetService<ScreenManager>().SetScreen(typeof(GameModeLoadingScreen));
        }

        private void OnLoadGame(LeftSideButton button)
        {
            // Game.ScreenManager.SetScreen(typeof(LoadGameScreen), enterInformation: this);
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.LightGray);
            _mainOverlay.Draw(GameInstance.GetService<SpriteBatch>());
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
            _mainOverlay.Update(gameTime);
        }

        public void OnClosing()
        {

        }
    }
}
