﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Common.Extensions;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.Transitions
{
    internal class SlideTransition : IScreenTransition
    {
        private Texture2D _source;
        private Texture2D _target;
        private float _elapsedTime;
        private float _transitionTime;
        
        public void StartTransition(Texture2D source, Texture2D target)
        {
            _source = source;
            _target = target;
            IsFinished = false;
            _elapsedTime = 0.0f;
            _transitionTime = 0.4f;
        }

        public bool IsFinished { get; private set; }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;
            _elapsedTime += gameTime.GetSeconds();

            if (_elapsedTime >= _transitionTime)
            {
                IsFinished = true;
                _elapsedTime = _transitionTime;
            }
        }

        public void Draw()
        {
            var bounds = GameInstance.GetService<Window>().ScreenBounds;

            var offset = (_elapsedTime/_transitionTime)* bounds.Width;
            var spriteBatch = GameInstance.GetService<SpriteBatch>();

            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);
            spriteBatch.Draw(_source, new Vector2(-offset, 0.0f), Color.White);
            spriteBatch.Draw(_target, new Vector2(bounds.Width - offset, 0.0f), Color.White);
            spriteBatch.End();
        }
    }
}
