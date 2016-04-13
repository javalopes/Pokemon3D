using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Common.Shapes;
using Microsoft.Xna.Framework;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Draws a bar to the bottom part of the screen that can display button functions for the current screen.
    /// </summary>
    class ControlBar
    {
        private class BarEntry
        {
            public string Text { get; set; }

            public Buttons GamePadButton { get; set; }

            public Keys KeyboardKey { get; set; }

            public Texture2D GetTexture()
            {
                switch (GamePadButton)
                {
                    case Buttons.Start:
                        return GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Menu);
                    case Buttons.A:
                        return GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A);
                    case Buttons.B:
                        return GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_B);
                    case Buttons.X:
                        return GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_X);
                    case Buttons.Y:
                        return GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Y);
                }

                return null;
            }
        }

        private readonly SpriteBatch _batch;
        private readonly List<BarEntry> _entries = new List<BarEntry>();
        private readonly ShapeRenderer _renderer;
        private readonly SpriteFont _font;
        private readonly Color _highlightColor;

        public ControlBar()
        {
            _batch = new SpriteBatch(GameInstance.GraphicsDevice);
            _renderer = new ShapeRenderer(_batch);
            _font = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _highlightColor = new Color(100, 193, 238);
        }

        public void AddEntry(string text, Buttons gamePadButton, Keys keyboardKey)
        {
            _entries.Add(new BarEntry()
            {
                Text = text,
                GamePadButton = gamePadButton,
                KeyboardKey = keyboardKey
            });
        }

        public void Draw()
        {
            _batch.Begin();

            _renderer.DrawRectangle(0, GameInstance.ScreenBounds.Height - 64, GameInstance.ScreenBounds.Width, 64, Color.White);

            int offset = 11;

            foreach (var entry in _entries)
            {
                if (GameInstance.InputSystem.GamePad.IsConnected())
                {
                    _batch.Draw(entry.GetTexture(), new Rectangle(offset, GameInstance.ScreenBounds.Height - 48, 32, 32), _highlightColor);
                    offset += 32;
                }
                else
                {
                    int boxWidth = 32;
                    string displayString = entry.KeyboardKey.ToString();

                    if (_font.MeasureString(displayString).X + 10 > 32)
                    {
                        boxWidth = (int)(_font.MeasureString(displayString).X + 10);
                    }

                    _renderer.DrawRectangle(new Rectangle(offset, GameInstance.ScreenBounds.Height - 48, boxWidth, 32), _highlightColor, filled: false);
                    _batch.DrawString(_font, displayString, new Vector2(offset + 5, GameInstance.ScreenBounds.Height - 48), _highlightColor);

                    offset += boxWidth;
                }

                _batch.DrawString(_font, entry.Text, new Vector2(offset + 10, GameInstance.ScreenBounds.Height - 48), _highlightColor);
                offset += 26 + (int)_font.MeasureString(entry.Text).X;
            }

            _batch.End();
        }
    }
}
