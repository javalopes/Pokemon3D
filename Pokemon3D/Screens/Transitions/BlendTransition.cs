using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.Transitions
{
    class BlendTransition : ScreenTransition
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
            _transitionTime = 1.0f;
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
            var alpha = _elapsedTime/_transitionTime;

            GameInstance.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            GameInstance.SpriteBatch.Draw(_target, Vector2.Zero, Color.White * alpha);
            GameInstance.SpriteBatch.Draw(_source, Vector2.Zero, Color.White * (1.0f - alpha));

            GameInstance.SpriteBatch.End();
        }
    }
}
