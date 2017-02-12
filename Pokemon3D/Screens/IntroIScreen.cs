using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Content;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Rendering.UI.Controls;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens
{
    internal class IntroIScreen : IScreenWithOverlays
    {
        public override void OnLateDraw(GameTime gameTime)
        {
            IGameInstance.GetService<GraphicsDevice>().Clear(Color.Black);
            base.OnLateDraw(gameTime);
        }

        public override void OnOpening(object enterInformation)
        {
            var overlay = AddOverlay(new UiOverlay());
            var logoSprite = overlay.AddElement(new Image(IGameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.SquareLogo_256px)));
            logoSprite.SetPosition(new Vector2(IGameInstance.ScreenBounds.Width * 0.5f, IGameInstance.ScreenBounds.Height * 0.5f));
            logoSprite.SetOriginPercentage(new Vector2(0.5f));

            var highlightSprite = overlay.AddElement(new Image(IGameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.highlight)));
            highlightSprite.Alpha = 0.0f;
            highlightSprite.AddCustomAnimation("Highlight", new UiCustomDeltaAnimation(1.5f, OnUpdateHighlightPass));
            highlightSprite.CustomAnimationFinshed += CustomAnimationFinished;
            highlightSprite.SetPosition(new Vector2(IGameInstance.ScreenBounds.Width * 0.5f+30, IGameInstance.ScreenBounds.Height * 0.5f-160));

            logoSprite.EnterAnimation = new UiMultiAnimation(new UiAnimation[]
            {
                new UiAlphaAnimation(1.0f, 0.0f, 1.0f),
                new UiRotationAnimation(1.0f, 0.0f, MathHelper.TwoPi), 
            });
            
            overlay.Showed += () => highlightSprite.PlayCustomAnimation("Highlight");
            overlay.Show();
        }

        private static void CustomAnimationFinished(string name)
        {
            IGameInstance.GetService<ScreenManager>().SetScreen(typeof(MainMenuIScreen));
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
