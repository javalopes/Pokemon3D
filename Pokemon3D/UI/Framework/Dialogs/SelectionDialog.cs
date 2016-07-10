using System;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework.Dialogs
{
    class SelectionDialog : UiCompoundElement
    {
        private readonly string _title;
        private readonly string _text;

        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _textFont;
        
        private int _calculatedHeight;

        public SelectionDialog(string title, string text, LeftSideButton[] buttons)
        {
            foreach (var leftSideButton in buttons)
            {
                AddChildElement(leftSideButton);
            }

            _titleFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _textFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            _title = title;
            _text = text;

            SetupLayout();

            GameInstance.WindowSizeChanged += HandleWindowSizeChanged;
        }

        private void HandleWindowSizeChanged(object sender, EventArgs e)
        {
            SetupLayout();
        }

        private void SetupLayout()
        {
            var totalHeight = 75;
            if (Children.Count > 0) totalHeight += Children.Sum(uiElement => 20 + uiElement.GetBounds().Height);

            _calculatedHeight = totalHeight;

            var controlY = 65;

            if (!string.IsNullOrWhiteSpace(_text))
            {
                var textSpace = (int)_textFont.MeasureString(_text).Y + 20;

                controlY += textSpace;
                _calculatedHeight += textSpace;
            }

            var startY = GameInstance.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;
            foreach (var uiElement in Children)
            {
                uiElement.SetPosition(new Vector2(120, controlY + startY));
                controlY += uiElement.GetBounds().Height + 20;
            }
        }

        public override bool IsInteractable => true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var startY = GameInstance.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;

            GameInstance.ShapeRenderer.DrawRectangle(0, 0, GameInstance.ScreenBounds.Width, GameInstance.ScreenBounds.Height, Color.White * 0.4f);
            GameInstance.ShapeRenderer.DrawRectangle(0, startY, GameInstance.ScreenBounds.Width, _calculatedHeight, new Color(251, 251, 251));

            spriteBatch.DrawString(_titleFont, _title, new Vector2(100, startY + 20), Color.Black);

            if (!string.IsNullOrWhiteSpace(_text)) spriteBatch.DrawString(_textFont, _text, new Vector2(120, startY + 65), Color.Black);
            
            base.Draw(spriteBatch);
        }
    }
}
