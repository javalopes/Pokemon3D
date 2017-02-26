using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common;
using Pokemon3D.Content;
using Pokemon3D.Rendering.UI;
using Pokemon3D.Common.Localization;
using Pokemon3D.Rendering.Shapes;
using static Pokemon3D.GameCore.GameProvider;

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

            _titleFont = GameInstance.GetService<ContentManager>().Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _textFont = GameInstance.GetService<ContentManager>().Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            _title = title;
            _text = text;

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

            var bounds = GameInstance.GetService<Window>().ScreenBounds;

            var startY = bounds.Height / 2 - _calculatedHeight / 2 - 35;
            foreach (var uiElement in Children)
            {
                uiElement.SetPosition(new Vector2(120, controlY + startY));
                controlY += uiElement.GetBounds().Height + 20;
            }
        }

        public override bool IsInteractable => true;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GameInstance.GetService<Window>().ScreenBounds;
            var startY = bounds.Height / 2 - _calculatedHeight / 2 - 35;

            var shapeRenderer = GameInstance.GetService<ShapeRenderer>();
            shapeRenderer.DrawRectangle(0, 0, bounds.Width, bounds.Height, Color.White * 0.4f);
            shapeRenderer.DrawRectangle(0, startY, bounds.Width, _calculatedHeight, new Color(251, 251, 251));

            spriteBatch.DrawString(_titleFont, _title.Value, new Vector2(100, startY + 20), Color.Black);

            if (!string.IsNullOrWhiteSpace(_text.Value)) spriteBatch.DrawString(_textFont, _text.Value, new Vector2(120, startY + 65), Color.Black);
            
            base.Draw(spriteBatch);
        }
    }
}
