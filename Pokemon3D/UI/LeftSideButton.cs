using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;

namespace Pokemon3D.UI
{
    internal class LeftSideButton : UiElement
    {
        private readonly SpriteFont _font;
        private readonly Action<LeftSideButton> _onClick;
        private readonly Texture2D _texture;

        public string Text { get; set; }

        public LeftSideButton(string text, Vector2 position, Action<LeftSideButton> onClick)
        {
            _font = GameProvider.GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);
            _texture = GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);

            Text = text;
            var bounds = Bounds;
            bounds.X = (int) position.X;
            bounds.Y = (int) position.Y;
            bounds.Width = 200;
            bounds.Height = 38;
            Bounds = bounds;
            _onClick = onClick;

            FocusedAnimation = new UiMultiAnimation(new UiAnimation[]
            {
                new UiColorAnimation(0.3f, new Color(255, 255, 255), new Color(100, 193, 238)),
                new UiOffsetAnimation(0.3f, Vector2.Zero, new Vector2(50,0))
            });
        }

        public override void OnAction()
        {
            _onClick?.Invoke(this);
        }

        public override bool IsInteractable => true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GetBounds();
            spriteBatch.Draw(_texture, bounds, null, Color * Alpha, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
            spriteBatch.DrawString(_font, Text, new Vector2(bounds.X + 24, bounds.Y + 5), Color.Black);
        }
    }
}
