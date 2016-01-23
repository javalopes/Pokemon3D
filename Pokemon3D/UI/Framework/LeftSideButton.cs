using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    class LeftSideButton : Control
    {
        private Texture2D _texture;
        private SpriteFont _font;

        private Vector2 _initialPosition;
        private Action<Control> _onClick;

        public string Text { get; set; }

        private ColorTransition _colorStepper;
        private OffsetTransition _offsetStepper;

        public bool Enabled { get; set; } = true;

        public LeftSideButton(ControlGroup group, string text, Vector2 position, Action<Control> onClick) : base(group)
        {
            _texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);
            _font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

            Text = text;
            _initialPosition = position;
            _onClick = onClick;

            _colorStepper = new ColorTransition(new Color(255, 255, 255), 0.5f);
            _offsetStepper = new OffsetTransition(0f, 0.5f);
        }

        public LeftSideButton(string text, Vector2 position, Action<Control> onClick) : this(null, text, position, onClick)
        { }

        public override Rectangle GetBounds()
        {
            return new Rectangle((int)(_initialPosition.X + _offsetStepper.Offset), (int)_initialPosition.Y, 200, 38);
        }

        public override void Update()
        {
            base.Update();

            if (_onClick != null && Selected)
            {
                if (Game.InputSystem.Accept(AcceptInputTypes.Buttons) ||
                    GetBounds().Contains(Game.InputSystem.Mouse.Position) && Game.InputSystem.Accept(AcceptInputTypes.LeftClick))
                {
                    _onClick(this);
                }
            }

            _offsetStepper.Update();
            _colorStepper.Update();
        }

        public override void Select()
        {
            base.Select();

            _offsetStepper.TargetOffset = 26;
            if (Enabled)
            {
                _colorStepper.TargetColor = new Color(100, 193, 238);
            }
            else
            {
                _colorStepper.TargetColor = new Color(210, 210, 210);
            }
        }

        public override void Deselect()
        {
            base.Deselect();

            _offsetStepper.TargetOffset = 0;
            _colorStepper.TargetColor = new Color(255, 255, 255);
        }

        public override void Draw()
        {
            Game.SpriteBatch.Draw(_texture, GetBounds(), _colorStepper.Color);
            Game.SpriteBatch.DrawString(_font, Text, new Vector2(_initialPosition.X + _offsetStepper.Offset + 24, _initialPosition.Y + 5), Color.Black);
        }
    }
}
