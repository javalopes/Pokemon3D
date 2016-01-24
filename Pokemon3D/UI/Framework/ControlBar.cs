using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Common;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Draws a bar to the bottom part of the screen that can display button functions for the current screen.
    /// </summary>
    class ControlBar : GameObject
    {
        private class BarEntry : GameObject
        {
            public string Text { get; set; }

            public Buttons GamePadButton { get; set; }

            public Keys KeyboardKey { get; set; }

            public Texture2D GetTexture()
            {
                switch (GamePadButton)
                {
                    case Buttons.Start:
                        return Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Menu);
                    case Buttons.A:
                        return Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A);
                    case Buttons.B:
                        return Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_B);
                    case Buttons.X:
                        return Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_X);
                    case Buttons.Y:
                        return Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_Y);
                }
                
                return null;
            }
        }

        private SpriteBatch _batch;
        private List<BarEntry> _entries = new List<BarEntry>();
        private ShapeRenderer _renderer;
        private SpriteFont _font;
        private Color _highlightColor;

        public ControlBar()
        {
            _batch = new SpriteBatch(Game.GraphicsDevice);
            _renderer = new ShapeRenderer(_batch, Game.GraphicsDevice);
            _font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
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

            _renderer.DrawFilledRectangle(0, Game.ScreenBounds.Height - 64, Game.ScreenBounds.Width, 64, Color.White);

            int offset = 11;

            foreach (var entry in _entries)
            {
                if (Game.InputSystem.GamePad.IsConnected())
                {
                    _batch.Draw(entry.GetTexture(), new Rectangle(offset, Game.ScreenBounds.Height - 48, 32, 32), _highlightColor);
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

                    _renderer.DrawRectangle(new Rectangle(offset, Game.ScreenBounds.Height - 48, boxWidth, 32), _highlightColor);
                    _batch.DrawString(_font, displayString, new Vector2(offset + 5, Game.ScreenBounds.Height - 48), _highlightColor);

                    offset += boxWidth;
                }

                _batch.DrawString(_font, entry.Text, new Vector2(offset + 10, Game.ScreenBounds.Height - 48), _highlightColor);
                offset += 26 + (int)_font.MeasureString(entry.Text).X;
            }

            _batch.End();
        }
    }
}
