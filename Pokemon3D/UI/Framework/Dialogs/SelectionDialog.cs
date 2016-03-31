using System;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework;
using Pokemon3D.Common.Shapes;
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

        private int _calculatedHeight; 

        private RenderTarget2D _target;

        public SelectionDialog(string title, string text, LeftSideButton[] buttons)
        {
            AddRange(buttons);

            _batch = new SpriteBatch(Game.GraphicsDevice);
            _renderer = new ShapeRenderer(_batch);
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
            if (Visible)
                SetupLayout();
        }

        private void SetupLayout()
        {
            int totalHeight = 75;
            if (Controls.Count > 0)
            {
                foreach (var control in Controls)
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
            foreach (var control in Controls)
            {
                control.SetPosition(new Vector2(120, controlY + startY));
                controlY += control.GetBounds().Height + 20;
            }

            _target = new RenderTarget2D(Game.GraphicsDevice, Game.ScreenBounds.Width, Game.ScreenBounds.Height);
        }

        public override void Draw(SamplerState samplerState = null, BlendState blendState = null)
        {
            if (Visible)
            {
                Game.GraphicsDevice.SetRenderTarget(_target);
                Game.GraphicsDevice.Clear(Color.Transparent);

                _batch.Begin();

                DrawDialog();

                _batch.End();

                Game.GraphicsDevice.SetRenderTarget(null);

                _batch.Begin(blendState: BlendState.NonPremultiplied);
                _batch.Draw(_target, Vector2.Zero, _colorStepper.Color);
                _batch.End();
            }
        }

        private void DrawDialog()
        {
            int startY = Game.ScreenBounds.Height / 2 - _calculatedHeight / 2 - 35;

            _renderer.DrawRectangle(0, 0, Game.ScreenBounds.Width, Game.ScreenBounds.Height, new Color(255, 255, 255, 100));
            _renderer.DrawRectangle(0, startY, Game.ScreenBounds.Width, _calculatedHeight, new Color(251, 251, 251));

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
            SetSelection(0);
            _colorStepper.TargetColor = Color.White;
            SetupLayout();
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
                if (Game.InputSystem.Up(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
                    MoveSelection(-1);
                if (Game.InputSystem.Down(InputDetectionType.PressedOnce, DirectionalInputTypes.All))
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
