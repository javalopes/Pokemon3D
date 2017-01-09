using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.Rendering.UI;
using Pokemon3D.UI;
using Pokemon3D.UI.Dialogs;

namespace Pokemon3D.Screens
{
    internal class MainMenuScreen : ScreenWithOverlays
    {
        private SelectionDialog _closeDialog;

        public override void OnOpening(object enterInformation)
        {
            var graphicsDeviceManager = GameProvider.GameInstance.GetService<GraphicsDeviceManager>();
            graphicsDeviceManager.PreferMultiSampling = true;
            graphicsDeviceManager.ApplyChanges();

            var mainOverlay = AddOverlay(new UiOverlay());

            var translation = GameProvider.GameInstance.GetService<TranslationProvider>();

            mainOverlay.AddElement(new HexagonBackground());
            mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "StartNewGame"), new Vector2(26, 45), OnStartNewGame));
            mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "LoadGame"), new Vector2(26, 107), OnLoadGame));
            mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "Gamejolt"), new Vector2(26, 169), null));
            mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "Options"), new Vector2(26, 231), null));
            mainOverlay.AddElement(new LeftSideButton(translation.CreateValue("System", "QuitGame"), new Vector2(26, 293), b => mainOverlay.ShowModal(_closeDialog)));

            mainOverlay.AddInputController(new KeyboardUiInputController());
            mainOverlay.AutoEnumerateTabIndices();

            _closeDialog = new SelectionDialog(translation.CreateValue("System", "ReallyQuitGameQuestion"), translation.CreateValue("System", "UnsavedChangesLost"), new[]
            {
                new LeftSideButton(translation.CreateValue("System", "No"), new Vector2(50, 50), b =>
                {
                    mainOverlay.CloseModal();
                }),
                new LeftSideButton(translation.CreateValue("System", "Yes"), new Vector2(50, 100), b =>
                {
                    GameProvider.GameInstance.GetService<ScreenManager>().NotifyQuitGame();
                })
            });

            var bar = new ControlBar();
            bar.AddEntry(translation.CreateValue("System", "Select"), Buttons.A, Keys.Enter);
            mainOverlay.AddElement(bar);
            mainOverlay.Show();
        }

        private void OnStartNewGame(LeftSideButton button)
        {
            GameProvider.GameInstance.GetService<ScreenManager>().SetScreen(typeof(GameModeLoadingScreen));
        }

        private void OnLoadGame(LeftSideButton button)
        {
            // Game.ScreenManager.SetScreen(typeof(LoadGameScreen), enterInformation: this);
        }

        public override void OnLateDraw(GameTime gameTime)
        {
            GameProvider.GameInstance.GraphicsDevice.Clear(Color.LightGray);
            base.OnLateDraw(gameTime);
        }
    }
}
