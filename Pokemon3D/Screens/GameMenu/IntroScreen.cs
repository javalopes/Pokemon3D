using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Rendering.UI.Controls;
using Pokemon3D.Screens.MainMenu;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.GameMenu
{
    internal class IntroScreen : Screen
    {
        private UiOverlay _overlay;

        public void OnEarlyDraw(GameTime gameTime)
        {
        }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.GraphicsDevice.Clear(Color.Black);
            _overlay.Draw(GameInstance.GetService<SpriteBatch>());
        }

        public void OnUpdate(GameTime gameTime)
        {
            _overlay.Update(gameTime);
        }

        public void OnClosing()
        {
        }

        public void OnOpening(object enterInformation)
        {
            _overlay = new UiOverlay();
            var logoSprite = _overlay.AddElement(new Image(GameInstance.Content.Load<Texture2D>(ResourceNames.Windows.Textures.SquareLogo_256px)));
            logoSprite.SetPosition(new Vector2(GameInstance.ScreenBounds.Width * 0.5f, GameInstance.ScreenBounds.Height * 0.5f));
            logoSprite.SetOriginPercentage(new Vector2(0.5f));

            var highlightSprite = _overlay.AddElement(new Image(GameInstance.Content.Load<Texture2D>(ResourceNames.Windows.Textures.highlight)));
            highlightSprite.Alpha = 0.0f;
            highlightSprite.AddCustomAnimation("Highlight", new UiCustomDeltaAnimation(1.5f, OnUpdateHighlightPass));
            highlightSprite.CustomAnimationFinshed += CustomAnimationFinished;
            highlightSprite.SetPosition(new Vector2(GameInstance.ScreenBounds.Width * 0.5f+30, GameInstance.ScreenBounds.Height * 0.5f-160));

            logoSprite.EnterAnimation = new UiMultiAnimation(new UiAnimation[]
            {
                new UiAlphaAnimation(1.0f, 0.0f, 1.0f),
                new UiRotationAnimation(1.0f, 0.0f, MathHelper.TwoPi), 
            });
            
            _overlay.Showed += () => highlightSprite.PlayCustomAnimation("Highlight");
            _overlay.Show();
        }

        private static void CustomAnimationFinished(string name)
        {
            GameInstance.GetService<ScreenManager>().SetScreen(typeof(MainMenuScreen));
        }

        private static void OnUpdateHighlightPass(UiElement owner, float delta)
        {
            var localDelta = (float) Math.Sin(Math.PI*delta);

            owner.Alpha = localDelta;
            owner.Scale = new Vector2(1.0f + localDelta);
            owner.Offset = new Vector2(30, 0.0f)*delta;
        }
    }
}
