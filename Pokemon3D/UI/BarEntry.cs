using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Localization;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Content;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    internal class BarEntry : UiElement
    {
        private readonly SpriteFont _font;
        private readonly Color _highlightColor;
        public BarEntry()
        {
            _font = GameProvider.GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _highlightColor = new Color(100, 193, 238);
        }

        public LocalizedValue Text { get; set; }
        public Buttons GamePadButton { get; set; }
        public Keys KeyboardKey { get; set; }
        public int Index { get; set; }

        public Texture2D GetTexture()
        {
            switch (GamePadButton)
            {
                case Buttons.Start:
                    return GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Menu);
                case Buttons.A:
                    return GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A);
                case Buttons.B:
                    return GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_B);
                case Buttons.X:
                    return GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_X);
                case Buttons.Y:
                    return GameProvider.GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Y);
            }

            return null;
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var offset = 11 + Index*(26 + (int) _font.MeasureString(Text.Value).X);

            if (GameProvider.GameInstance.GetService<InputSystem>().GamePadHandler.IsConnected())
            {
                spriteBatch.Draw(GetTexture(), new Rectangle(offset, GameProvider.GameInstance.ScreenBounds.Height - 48, 32, 32), _highlightColor);
                offset += 32;
            }
            else
            {
                var boxWidth = 32;
                var displayString = KeyboardKey.ToString();

                if (_font.MeasureString(displayString).X + 10 > 32)
                {
                    boxWidth = (int)(_font.MeasureString(displayString).X + 10);
                }

                GameProvider.GameInstance.GetService<ShapeRenderer>().DrawRectangle(new Rectangle(offset, GameProvider.GameInstance.ScreenBounds.Height - 48, boxWidth, 32), _highlightColor, filled: false);
                spriteBatch.DrawString(_font, displayString, new Vector2(offset + 5, GameProvider.GameInstance.ScreenBounds.Height - 48), _highlightColor);

                offset += boxWidth;
            }

            spriteBatch.DrawString(_font, Text.Value, new Vector2(offset + 10, GameProvider.GameInstance.ScreenBounds.Height - 48), _highlightColor);
        }
    }
}