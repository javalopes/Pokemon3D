using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.FileSystem;

namespace Pokemon3D.UI.Screens
{
    class LoadGameScreen : GameObject, Screen
    {
        private Screen _preScreen;

        public void OnOpening(object enterInformation)
        {
            _preScreen = (Screen)enterInformation;

            // create save directory, if it doesn't exist:
            if (!Directory.Exists(StaticPathProvider.SavePath))
                Directory.CreateDirectory(StaticPathProvider.SavePath);
        }

        public void OnDraw(GameTime gameTime)
        {
            _preScreen.OnDraw(gameTime);

            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            
            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {

        }

        public void OnClosing()
        {

        }
    }
}
