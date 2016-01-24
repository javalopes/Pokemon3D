using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Common.Input;
using Pokemon3D.UI.Transitions;
using Pokemon3D.UI.Framework;
using Pokemon3D.UI.Framework.Dialogs;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.UI.Screens
{
    class MainMenuScreen2 : GameObject, Screen
    {
        private DefaultControlGroup _buttons;
        private SelectionDialog _closeDialog;

        private HexagonBackground _hexagons = new HexagonBackground();
        private ControlBar _bar = new ControlBar();
        
        public void OnOpening(object enterInformation)
        {
            Game.GraphicsDeviceManager.PreferMultiSampling = true;
            Game.GraphicsDeviceManager.ApplyChanges();

            _buttons = new DefaultControlGroup();
            _buttons.Add(new LeftSideButton("Start new game", new Vector2(26, 45), b =>
            {
                Game.ScreenManager.SetScreen(typeof(GameModeLoadingScreen), typeof(BlendTransition));
            }));
            _buttons.Add(new LeftSideButton("Load game", new Vector2(26, 107), b =>
            {
                Game.ScreenManager.SetScreen(typeof(LoadGameScreen), enterInformation: this);
            }));
            _buttons.Add(new LeftSideButton("GameJolt", new Vector2(26, 169), null));
            _buttons.Add(new LeftSideButton("Options", new Vector2(26, 231), null));
            _buttons.Add(new LeftSideButton("Exit game", new Vector2(26, 293), (b) =>
            {
                _closeDialog.Show();
            }));
            _buttons.Add(new LeftSideCheckbox("Checkbox test", new Vector2(26, 355), null));

            _closeDialog = new SelectionDialog("Do you really want to exit?", "Any unsaved changes will be lost.", new LeftSideButton[]
            {
                new LeftSideButton("No", new Vector2(50, 50), (b) =>
                {
                    _closeDialog.Close();
                }),
                new LeftSideButton("Yes", new Vector2(50, 100), (b) =>
                {
                    Game.ScreenManager.NotifyQuitGame();
                })
            }, 0);

            _closeDialog.Closed += HandleCloseDialogClosed;
            _closeDialog.Shown += HandleCloseDialogShown;

            _buttons.Active = true;
            _buttons.Visible = true;

            _bar.AddEntry("Select", Buttons.A, Keys.Enter);
        }
        
        private void HandleCloseDialogShown(ControlGroup sender)
        {
            _buttons.Active = false;
        }

        private void HandleCloseDialogClosed(ControlGroup sender)
        {
            _buttons.Active = true;
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.LightGray);
            
            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            _hexagons.Draw(Game.SpriteBatch);

            Game.SpriteBatch.End();
            
            _bar.Draw();

            _buttons.Draw();

            _closeDialog.Draw();
        }

        public void OnUpdate(float elapsedTime)
        {
            _closeDialog.Update();
            _buttons.Update();
            _hexagons.Update();
        }

        public void OnClosing()
        {

        }
    }
}
