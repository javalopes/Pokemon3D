using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.UI.Screens
{
    class OverlayScreen : GameObject, Screen
    {
        Screen _preScreen;
        HexagonBackground _hexagons;

        public void OnOpening(object enterInformation)
        {
            _preScreen = (Screen)enterInformation;
            _hexagons = new HexagonBackground();
        }

        public void OnDraw(GameTime gameTime)
        {
            _preScreen.OnDraw(gameTime);

            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            _hexagons.Draw(Game.SpriteBatch);

            Game.SpriteBatch.End();

        }

        public void OnUpdate(float elapsedTime)
        {
            _hexagons.Update();
        }

        public void OnClosing()
        {

        }
    }
}
