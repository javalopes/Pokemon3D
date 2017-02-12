using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Content;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Rendering.UI.Animations;
using Pokemon3D.Common.Localization;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI
{
    internal class LeftSideButton : UiElement
    {
        private readonly SpriteFont _font;
        private readonly Action<LeftSideButton> _onClick;
        private readonly Texture2D _texture;

        public LocalizedValue Text { get; set; }

        public LeftSideButton(LocalizedValue text, Vector2 position, Action<LeftSideButton> onClick)
        {
            _font = IGameInstance.GetService<ContentManager>().Load<SpriteFont>(ResourceNames.Fonts.NormalFont);
            _texture = IGameInstance.GetService<ContentManager>().Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);

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
            spriteBatch.DrawString(_font, Text.Value, new Vector2(bounds.X + 24, bounds.Y + 5), Color.Black);
        }
    }
}
