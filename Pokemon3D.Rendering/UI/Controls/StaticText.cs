using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Rendering.UI.Controls
{
    public class StaticText : UiElement
    {
        private readonly SpriteFont _font;
        private string _text;

        public StaticText(SpriteFont spriteFont, string text = null)
        {
            _font = spriteFont;
            Text = text;
            MeasureBounds();
        }

        private void MeasureBounds()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                Bounds = new Rectangle();
            }
            else
            {
                var measure = _font.MeasureString(Text);
                var bounds = Bounds;
                bounds.Width = (int) measure.X;
                bounds.Height = (int) measure.Y;
                Bounds = bounds;
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    MeasureBounds();
                }
            }
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrWhiteSpace(Text)) return;
            var bounds = GetBounds();
            spriteBatch.DrawString(_font, Text, new Vector2(bounds.X, bounds.Y), Color * Alpha, 0.0f, Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
