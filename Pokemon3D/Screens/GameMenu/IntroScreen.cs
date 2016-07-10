using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Screens.MainMenu;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.GameMenu
{
    class IntroScreen : Screen
    {
        //private Animator _logoAnimator;

        //private Sprite _logoSprite;
        //private Sprite _highlightSprite;

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.Black);
            GameInstance.SpriteBatch.Begin();

            //_logoSprite.Draw(GameInstance.SpriteBatch);
            //_highlightSprite.Draw(GameInstance.SpriteBatch);
            GameInstance.SpriteBatch.End();
        }

        public void OnUpdate(GameTime gameTime)
        {
            //_logoAnimator.Update(gameTime);
        }

        public void OnClosing()
        {
        }

        public void OnOpening(object enterInformation)
        {
            //_logoSprite = new Sprite(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.SquareLogo_256px))
            //{
            //    Position = new Vector2(GameInstance.ScreenBounds.Width * 0.5f, GameInstance.ScreenBounds.Height * 0.5f)
            //};

            //_highlightSprite = new Sprite(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.highlight))
            //{
            //    Alpha = 0.0f
            //};

            //_logoAnimator = new Animator();
            //_logoAnimator.AddAnimation("TurningAlpha", Animation.CreateDelta(1.0f, OnUpdateTurningAlpha));
            //_logoAnimator.AddAnimation("Wait", Animation.CreateWait(0.5f));
            //_logoAnimator.AddAnimation("HighlightPass", Animation.CreateDelta(1.5f, OnUpdateHighlightPass));
            //_logoAnimator.AddTransitionChain("TurningAlpha", "Wait", "HighlightPass");
            //_logoAnimator.SetAnimation("TurningAlpha");
            //_logoAnimator.AnimatorFinished += OnAnimatorFinished;
        }

        private void OnAnimatorFinished()
        {
            GameInstance.ScreenManager.SetScreen(typeof(MainMenuScreen));
        }

        private void OnUpdateHighlightPass(float delta)
        {
            //if (delta <= 0.5f)
            //{
            //    _highlightSprite.Alpha = MathHelper.SmoothStep(0.0f, 1.0f, delta * 2.0f);
            //    _highlightSprite.Scale = new Vector2(MathHelper.SmoothStep(1.0f, 2.0f, delta * 2.0f));
            //}
            //else
            //{
            //    _highlightSprite.Alpha = MathHelper.SmoothStep(1.0f, 0.0f, (delta - 0.5f) * 2.0f);
            //    _highlightSprite.Scale = new Vector2(MathHelper.SmoothStep(2.0f, 1.0f, (delta - 0.5f) * 2.0f));
            //}

            //_highlightSprite.Position = _logoSprite.Position + new Vector2(0.0f, -_logoSprite.Height * 0.5f) + new Vector2(_logoSprite.Width * 0.5f, 0.0f) * delta;
        }

        private void OnUpdateTurningAlpha(float delta)
        {
            //_logoSprite.Rotation = MathHelper.SmoothStep(0.0f, MathHelper.TwoPi, delta);
            //_logoSprite.Alpha = MathHelper.SmoothStep(0.0f, 1.0f, delta);
            //_logoSprite.Scale = new Vector2(MathHelper.SmoothStep(0.5f, 1.0f, delta));
        }
    }
}
