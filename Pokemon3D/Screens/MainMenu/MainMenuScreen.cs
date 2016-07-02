using Microsoft.Xna.Framework;
using Pokemon3D.Screens.Transitions;
using Pokemon3D.UI.Framework;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Screens.GameModeLoading;
using Pokemon3D.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.MainMenu
{
    class MainMenuScreen : Screen
    {
        private UiOverlay _mainOverlay;
        //private SelectionDialog _closeDialog;

        private readonly ControlBar _bar = new ControlBar();

        public void OnOpening(object enterInformation)
        {
            GameInstance.GraphicsDeviceManager.PreferMultiSampling = true;
            GameInstance.GraphicsDeviceManager.ApplyChanges();

            _mainOverlay = new UiOverlay();

            _mainOverlay.AddElement(new HexagonBackground());
            _mainOverlay.AddElement(new LeftSideButton("Start new game", new Vector2(26, 45), OnStartNewGame));
            _mainOverlay.AddElement(new LeftSideButton("Load game", new Vector2(26, 107), OnLoadGame));
            _mainOverlay.AddElement(new LeftSideButton("GameJolt", new Vector2(26, 169), null));
            _mainOverlay.AddElement(new LeftSideButton("Options", new Vector2(26, 231), null));
            _mainOverlay.AddElement(new LeftSideButton("Exit game", new Vector2(26, 293), b =>
            {
                //_closeDialog.Show();
            }));
            //_buttons.AddElement(new LeftSideCheckbox("Checkbox test", new Vector2(26, 355), null));

            _mainOverlay.AddInputController(new KeyboardUiInputController());
            _mainOverlay.AutoEnumerateTabIndices();

            //_closeDialog = new SelectionDialog("Do you really want to exit?", "Any unsaved changes will be lost.", new[]
            //{
            //    new LeftSideButton("No", new Vector2(50, 50), b =>
            //    {
            //        _closeDialog.Close();
            //    }),
            //    new LeftSideButton("Yes", new Vector2(50, 100), b =>
            //    {
            //        GameInstance.ScreenManager.NotifyQuitGame();
            //    })
            //});
            _mainOverlay.Show();

            //_closeDialog.Closed += HandleCloseDialogClosed;
            //_closeDialog.Shown += HandleCloseDialogShown;

            //_buttons.Active = true;
            //_buttons.Visible = true;

            _bar.AddEntry("Select", Buttons.A, Keys.Enter);
        }

        private void OnStartNewGame(LeftSideButton button)
        {
            GameInstance.ScreenManager.SetScreen(typeof(GameModeLoadingScreen), typeof(BlendTransition));
        }

        private void OnLoadGame(LeftSideButton button)
        {
            // Game.ScreenManager.SetScreen(typeof(LoadGameScreen), enterInformation: this);
        }

        //private void HandleCloseDialogShown(ControlGroup sender)
        //{
        //    //_buttons.Active = false;
        //}

        //private void HandleCloseDialogClosed(ControlGroup sender)
        //{
        //    //_buttons.Active = true;
        //}

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.LightGray);
            _mainOverlay.Draw(GameInstance.SpriteBatch);
            _bar.Draw();

            //_closeDialog.Draw();
        }

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
            //_closeDialog.Update();
            _mainOverlay.Update(gameTime);
        }

        public void OnClosing()
        {

        }
    }
}
