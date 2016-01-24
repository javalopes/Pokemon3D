using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;

namespace Pokemon3D.UI.Screens
{
    class OverlayScreen : GameObject, Screen
    {
        Screen _preScreen;
        HexagonBackground _hexagons;
        ControlBar _bar;

        private PokemonSpriteSheet _sheet;

        public void OnOpening(object enterInformation)
        {
            _preScreen = (Screen)enterInformation;
            _hexagons = new HexagonBackground();
            _bar = new ControlBar();

            _bar.AddEntry("Select", Buttons.A, Keys.Enter);
            _bar.AddEntry("Back", Buttons.B, Keys.Escape);

            _sheet = new PokemonSpriteSheet(Game.ActiveGameMode.GetTexture("Pokemon\\Bulbasaur\\Default\\Front"), 49);
        }

        public void OnDraw(GameTime gameTime)
        {
            _preScreen.OnDraw(gameTime);

            _hexagons.Draw();

            _bar.Draw();

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(_sheet.CurrentFrame, new Rectangle(100, 100, 98, 98), Color.White);

            Game.SpriteBatch.End();
        }
        
        public void OnUpdate(float elapsedTime)
        {
            _hexagons.Update();

            _sheet.Update();

            if (Game.InputSystem.Dismiss(DismissInputTypes.BButton | DismissInputTypes.EscapeKey))
            {
                Game.ScreenManager.SetScreen(_preScreen.GetType());
            }
        }

        public void OnClosing()
        {

        }
    }
}
