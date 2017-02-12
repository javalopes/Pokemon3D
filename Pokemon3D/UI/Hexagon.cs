using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI
{
    internal class Hexagon : UiElement
    {
        public const int Width = 26;
        public const int Height = 31;
        public const int HeightHalf = 15;

        private const int MinAlpha = 150;
        private const int MaxAlpha = 220;
        private const float DelayVerticalOffsetMultiplier = 0.04f;
        private const int BlinkingChance = 40000;

        private readonly Texture2D _hexagonTexture;
        private readonly UiAlphaAnimation _changeAlphaAnimation;

        public override bool IsInteractable => false;

        public Hexagon(Texture2D texture, int x, int y, bool hasAnimation)
        {
            _hexagonTexture = texture;
            Bounds = new Rectangle(x * Width, y * Height - ((x % 2) * HeightHalf), texture.Width, texture.Height);
            var targetAlpha = GameInstance.GetService<Random>().Next(MinAlpha, MaxAlpha);

            if (hasAnimation)
            {
                EnterAnimation = new UiAlphaAnimation(0.5f, 0.0f, targetAlpha / 255.0f)
                {
                    Delay = Math.Max(y*DelayVerticalOffsetMultiplier, 0)
                };
                Alpha = 0.0f;
            }
            else
            {
                Alpha = targetAlpha/255.0f;
            }

            _changeAlphaAnimation = AddAnimation("ChangeAlpha", new UiAlphaAnimation(0.3f, 0, 1));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (State == UiState.Active && !IsAnimating)
            {
                if (GameInstance.GetService<Random>().Next(0, BlinkingChance) != 0) return;

                _changeAlphaAnimation.StartAlpha = Alpha;
                _changeAlphaAnimation.EndAlpha = GameInstance.GetService<Random>().Next(0, 2) == 0 ? 255 : 0;
                PlayAnimation("ChangeAlpha");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_hexagonTexture, Bounds, null, Color.White * Alpha);
        }
    }
}