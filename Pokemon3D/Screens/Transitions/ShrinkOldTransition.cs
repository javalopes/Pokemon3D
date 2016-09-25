using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Extensions;
using static GameProvider;

namespace Pokemon3D.Screens.Transitions
{
    internal class ShrinkOldTransition : ScreenTransition
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
            var spriteBatch = GameInstance.GetService<SpriteBatch>();
            var scale = 1.0f - _elapsedTime / _transitionTime;

            spriteBatch.Begin();
            spriteBatch.Draw(_target, Vector2.Zero, Color.White);
            spriteBatch.Draw(_source, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
