using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.Common;

namespace Pokemon3D.UI.Framework.Tablet
{
    class MainMenuButton : TabletControl
    {
        private Texture2D _backTexture, _outlineTexture;
        private Texture2D _buttonTexture;
        private SpriteFont _font;
        private Vector2 _position;
        private OffsetTransition _outerSizeStepper;
        private ColorTransition _colorStepper;
        private float _rotation = 0f;
        private string _text;
        private ShapeRenderer _renderer;

        public MainMenuButton(TextureProjectionQuad quad, Vector2 position, string buttonTextureResource, string text) : base(quad)
        {
            _position = position;
            _text = text;

            _outerSizeStepper = new OffsetTransition(0f, 0.6f);
            _colorStepper = new ColorTransition(Color.White, 0.6f);

            _backTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.MainMenu.ButtonBack);
            _outlineTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.MainMenu.ButtonLine);
            _buttonTexture = Game.Content.Load<Texture2D>(buttonTextureResource);
            _font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
        }

        private Rectangle GetBoundsInternal()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, 110, 118);
        }

        public override Rectangle GetBounds()
        {
            return _quad.AdjustToScreen(_quad.ProjectRectangle(GetBoundsInternal())).Bounds;
        }

        public override void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public override void Select()
        {
            base.Select();

            _outerSizeStepper.TargetOffset = 1f;
            _colorStepper.TargetColor = new Color(77, 186, 216);
        }

        public override void Deselect()
        {
            base.Deselect();

            _outerSizeStepper.TargetOffset = 0f;
            _colorStepper.TargetColor = Color.White;
        }

        public override void Update()
        {
            base.Update();

            _outerSizeStepper.Update();
            _colorStepper.Update();

            if (Selected)
            {
                _rotation += 0.03f;
            }
            else
            {
                if (_rotation > 0f)
                {
                    _rotation -= _rotation / 10f;
                    if (_rotation <= 0.01f)
                    {
                        _rotation = 0f;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_renderer == null)
                _renderer = new ShapeRenderer(spriteBatch, Game.GraphicsDevice);

            Color textColor = Color.White;
            Vector2 textSize = _font.MeasureString(_text);

            spriteBatch.Draw(_outlineTexture, GetBoundsInternal(), Color.White);
            if (Selected || _outerSizeStepper.Offset > 0f)
            {
                var bounds = GetBoundsInternal();

                int width = (int)Math.Floor(bounds.Width * _outerSizeStepper.Offset);
                int height = (int)Math.Floor(bounds.Height * _outerSizeStepper.Offset);

                var drawBounds = new Rectangle(bounds.X + (bounds.Width - width) / 2, bounds.Y + (bounds.Height - height) / 2, width, height);

                var cutRect = new Rectangle(bounds.Width / 2 - width / 2, bounds.Height / 2 - height / 2, width, height);

                spriteBatch.Draw(_backTexture, drawBounds, null, new Color(255, 255, 255, 255));

                textColor = new Color(77, 186, 216);
                _renderer.DrawFilledRectangle(new Rectangle((int)(_position.X + _outlineTexture.Width / 2 - textSize.X / 2 - 4), (int)(_position.Y + _outlineTexture.Height), (int)textSize.X + 8, (int)textSize.Y), Color.White);
            }

            spriteBatch.Draw(texture: _buttonTexture,
                position: _position + new Vector2(_outlineTexture.Width / 2, _outlineTexture.Height / 2),
                color: _colorStepper.Color,
                origin: new Vector2(32, 32),
                rotation: _rotation);

            spriteBatch.DrawString(_font, _text, new Vector2(_position.X + _outlineTexture.Width / 2 - textSize.X / 2, _position.Y + _outlineTexture.Height), textColor);
        }
    }
}
