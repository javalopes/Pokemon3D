using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Content;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Common.Localization;
using Pokemon3D.Rendering.Shapes;

namespace Pokemon3D.UI.Dialogs
{
    internal class SelectionDialog : UiCompoundElement
    {
        private readonly LocalizedValue _title;
        private readonly LocalizedValue _text;

        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _textFont;
        
        private int _calculatedHeight;

        public SelectionDialog(LocalizedValue title, LocalizedValue text, LeftSideButton[] buttons)
        {
            foreach (var leftSideButton in buttons)
            {
                AddChildElement(leftSideButton);
            }

            _titleFont = GameProvider.GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _textFont = GameProvider.GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            _title = title;
            _text = text;

            SetupLayout();

            GameProvider.GameInstance.WindowSizeChanged += HandleWindowSizeChanged;
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

            if (!string.IsNullOrWhiteSpace(_text.Value))
            {
                var textSpace = (int)_textFont.MeasureString(_text.Value).Y + 20;

                controlY += textSpace;
                _calculatedHeight += textSpace;
            }

            var startY = GameProvider.GameInstance.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;
            foreach (var uiElement in Children)
            {
                uiElement.SetPosition(new Vector2(120, controlY + startY));
                controlY += uiElement.GetBounds().Height + 20;
            }
        }

        public override bool IsInteractable => true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var startY = GameProvider.GameInstance.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;

            var shapeRenderer = GameProvider.GameInstance.GetService<ShapeRenderer>();
            shapeRenderer.DrawRectangle(0, 0, GameProvider.GameInstance.ScreenBounds.Width, GameProvider.GameInstance.ScreenBounds.Height, Color.White * 0.4f);
            shapeRenderer.DrawRectangle(0, startY, GameProvider.GameInstance.ScreenBounds.Width, _calculatedHeight, new Color(251, 251, 251));

            spriteBatch.DrawString(_titleFont, _title.Value, new Vector2(100, startY + 20), Color.Black);

            if (!string.IsNullOrWhiteSpace(_text.Value)) spriteBatch.DrawString(_textFont, _text.Value, new Vector2(120, startY + 65), Color.Black);
            
            base.Draw(spriteBatch);
        }
    }
}
