using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Localization;

namespace Pokemon3D.Rendering.UI.Controls
{
    public class StaticText : UiElement
    {
        private readonly SpriteFont _font;
        private LocalizedValue _text;

        public StaticText(SpriteFont spriteFont, LocalizedValue text)
        {
            _font = spriteFont;
            Text = text;
            MeasureBounds();
        }

        private void MeasureBounds()
        {
            if (string.IsNullOrWhiteSpace(Text.Value))
            {
                Bounds = new Rectangle();
            }
            else
            {
                var measure = _font.MeasureString(Text.Value);
                var bounds = Bounds;
                bounds.Width = (int) measure.X;
                bounds.Height = (int) measure.Y;
                Bounds = bounds;
            }
        }

        public LocalizedValue Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    if (_text != null) _text.ValueChanged -= OnValueChanged;
                    _text = value;
                    if (_text != null) _text.ValueChanged += OnValueChanged;
                    MeasureBounds();
                }
            }
        }

        private void OnValueChanged()
        {
            MeasureBounds();
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrWhiteSpace(Text.Value)) return;
            var bounds = GetBounds();
            spriteBatch.DrawString(_font, Text.Value, new Vector2(bounds.X, bounds.Y), Color * Alpha, 0.0f, Origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
