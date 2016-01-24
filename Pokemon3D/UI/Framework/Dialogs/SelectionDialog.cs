using System;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;
using Pokemon3D.Common.Input;

namespace Pokemon3D.UI.Framework.Dialogs
{
    class SelectionDialog : ModalDialog
    {
        private string _title;
        private string _text;
        private bool _closing = false;

        SpriteFont _titleFont;
        SpriteFont _textFont;

        SpriteBatch _batch;
        ColorTransition _colorStepper;
        ShapeRenderer _renderer;

        private int _calculatedHeight; // starting with 75, which is the height with only a header.

        public SelectionDialog(string title, string text, LeftSideButton[] buttons, int selectedIndex)
        {
            AddRange(buttons);
            SetSelection(selectedIndex);

            _batch = new SpriteBatch(Game.GraphicsDevice);
            _renderer = new ShapeRenderer(_batch, Game.GraphicsDevice);
            _colorStepper = new ColorTransition(new Color(255, 255, 255, 0), 0.7f);

            _titleFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
            _textFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            _title = title;
            _text = text;

            SetupLayout();

            Game.WindowSizeChanged += HandleWindowSizeChanged;
        }

        private void HandleWindowSizeChanged(object sender, EventArgs e)
        {
            SetupLayout();
        }

        private void SetupLayout()
        {
            int totalHeight = 75;
            if (_controls.Count > 0)
            {
                foreach (var control in _controls)
                {
                    totalHeight += 20 + control.GetBounds().Height;
                }
            }

            _calculatedHeight = totalHeight;

            int controlY = 65;

            if (!string.IsNullOrWhiteSpace(_text))
            {
                int textSpace = (int)_textFont.MeasureString(_text).Y + 20;

                controlY += textSpace;
                _calculatedHeight += textSpace;
            }

            int startY = Game.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;
            foreach (var control in _controls)
            {
                control.SetPosition(new Vector2(120, controlY  + startY));
                controlY += control.GetBounds().Height + 20;
            }
        }

        public override void Draw()
        {
            if (Visible)
            {
                var target = new RenderTarget2D(Game.GraphicsDevice, Game.ScreenBounds.Width, Game.ScreenBounds.Height);
                Game.GraphicsDevice.SetRenderTarget(target);
                 Game.GraphicsDevice.Clear(Color.Transparent);

                _batch.Begin();

                DrawDialog();

                _batch.End();

                Game.GraphicsDevice.SetRenderTarget(null);

                _batch.Begin(blendState: BlendState.NonPremultiplied);
                _batch.Draw(target, Vector2.Zero, _colorStepper.Color);
                _batch.End();
            }
        }

        private void DrawDialog()
        {
            int startY = Game.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;

            _renderer.DrawFilledRectangle(0, 0, Game.ScreenBounds.Width, Game.ScreenBounds.Height, new Color(255, 255, 255, 100));
            _renderer.DrawFilledRectangle(0, startY, Game.ScreenBounds.Width, _calculatedHeight, new Color(251, 251, 251));

            _batch.DrawString(_titleFont, _title, new Vector2(100, startY + 20), Color.Black);

            if (!string.IsNullOrWhiteSpace(_text))
            {
                _batch.DrawString(_textFont, _text, new Vector2(120, startY + 65), Color.Black);
            }

            InternalDraw(_batch);
        }

        public override void Show()
        {
            base.Show();
            _colorStepper.TargetColor = Color.White;
        }

        public override void Close()
        {
            Active = false;
            _closing = true;
            _colorStepper.TargetColor = new Color(255, 255, 255, 0);
        }

        public override void Update()
        {
            if (Active)
            {
                if (Game.InputSystem.Up(true, DirectionalInputTypes.All))
                    MoveSelection(-1);
                if (Game.InputSystem.Down(true, DirectionalInputTypes.All))
                    MoveSelection(1);
                base.Update();
            }
            
            _colorStepper.Update();

            if (_closing && _colorStepper.Finished)
            {
                _closing = false;
                Visible = false;
                OnClose();
            }
        }
    }
}
