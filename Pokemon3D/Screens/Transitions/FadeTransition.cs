using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Screens.Transitions
{
    class FadeTransition : GameObject, ScreenTransition
    {
        private Texture2D _source;
        private Texture2D _target;

        private int _alpha = 0;

        public bool IsFinished { get; private set; }

        public void StartTransition(Texture2D source, Texture2D target)
        {
            _source = source;
            _target = target;
            IsFinished = false;
        }

        public void Update(float elapsedTimeSeconds)
        {
            if (IsFinished) return;
            if (_alpha >= 255)
            {
                IsFinished = true;
            }
            else
            {
                _alpha += 10;
            }
        }

        public void Draw()
        {
            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            Game.SpriteBatch.Draw(_source, Vector2.Zero, Color.White);
            Game.SpriteBatch.Draw(_target, Vector2.Zero, new Color(255, 255, 255, _alpha));

            Game.SpriteBatch.End();
        }
    }
}
