using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.GameModes;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Transitions;
using System.Diagnostics;
using Pokemon3D.Common;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.UI.Screens
{
    class GameModeLoadingScreen : GameObject, Screen
    {
        private GameMode _gameMode;
        private Stopwatch _sw;
        private bool _loadingFinished;

        public void OnDraw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont), "Loading...", new Vector2(40, 40), Color.White);
            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {
            if (_loadingFinished)
            {
                _sw.Stop();
                Common.Diagnostics.GameLogger.Instance.Log(Common.Diagnostics.MessageType.Debug, "Loading time: " + _sw.ElapsedMilliseconds);
                Game.ScreenManager.SetScreen(typeof(OverworldScreen), typeof(SlideTransition));
            }
        }

        public void OnOpening(object enterInformation)
        {
            var gameModes = Game.GameModeManager.GetGameModeInfos();
            _gameMode = Game.GameModeManager.CreateGameMode(gameModes.First());
            Game.Resources.SetPrimitiveProvider(_gameMode);
            Game.ActiveGameMode = _gameMode;
            _loadingFinished = false;
            _sw = Stopwatch.StartNew();
            _gameMode.PreloadAsync(() => _loadingFinished = true);
        }

        public void OnClosing()
        {

        }
    }
}
