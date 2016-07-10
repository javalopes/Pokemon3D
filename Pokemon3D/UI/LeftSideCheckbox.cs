namespace Pokemon3D.UI
{
    //class LeftSideCheckbox : Control
    //{
    //    public string Text { get; set; }

    //    private Texture2D _texture;
    //    private Texture2D _backTexture;
    //    private Texture2D _markTexture;
    //    private SpriteFont _font;

    //    private Action<Control> _onClick;
    //    private Vector2 _position;

    //    private ColorTransition _colorStepper;
    //    private OffsetTransition _offsetStepper;
    //    private ColorTransition _markColorStepper;

    //    public bool Checked { get; set; }
    //    private int _checkSize = 0;

    //    public LeftSideCheckbox(string text, Vector2 position, Action<Control> onClick)
    //    {
    //        _texture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);
    //        _backTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Checkbox_Back);
    //        _markTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Checkbox_Mark);
    //        _font = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);
    //        _position = position;

    //        Text = text;
    //        _onClick = onClick;

    //        _colorStepper = new ColorTransition(Color.White, 0.5f);
    //        _markColorStepper = new ColorTransition(Color.Black, 0.5f);
    //        _offsetStepper = new OffsetTransition(0f, 0.5f);
    //    }

    //    public override Rectangle GetBounds()
    //    {
    //        if (Text.Length > 0)
    //        {
    //            return new Rectangle((int)(_position.X + _offsetStepper.Offset), (int)_position.Y, 200, 38);
    //        }
    //        else
    //        {
    //            return new Rectangle((int)(_position.X + _offsetStepper.Offset), (int)_position.Y, 42, 38);
    //        }
    //    }

    //    public override void Update()
    //    {
    //        base.Update();

    //        if (Selected && Group != null && Group.Active)
    //        {
    //            if (GameInstance.InputSystem.Accept(AcceptInputTypes.Buttons) ||
    //                GetBounds().Contains(GameInstance.InputSystem.Mouse.Position) && GameInstance.InputSystem.Accept(AcceptInputTypes.LeftClick))
    //            {
    //                Checked = !Checked;

    //                if (_onClick != null)
    //                {
    //                    _onClick(this);
    //                }
    //            }
    //        }

    //        if (Checked)
    //        {
    //            if (_checkSize < 15)
    //                _checkSize += 3;
    //        }
    //        else
    //        {
    //            if (_checkSize > 0)
    //                _checkSize -= 3;
    //        }

    //        _offsetStepper.Update();
    //        _colorStepper.Update();
    //        _markColorStepper.Update();
    //    }

    //    public override void Select()
    //    {
    //        base.Select();

    //        _offsetStepper.TargetOffset = 26;
    //        _colorStepper.TargetColor = new Color(100, 193, 238);
    //        _markColorStepper.TargetColor = Color.White;
    //    }

    //    public override void Deselect()
    //    {
    //        base.Deselect();

    //        _offsetStepper.TargetOffset = 0;
    //        _colorStepper.TargetColor = new Color(255, 255, 255);
    //        _markColorStepper.TargetColor = Color.Black;
    //    }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        if (Text.Length > 0)
    //        {
    //            spriteBatch.Draw(_texture, GetBounds(), _colorStepper.Color);
    //            spriteBatch.DrawString(_font, Text, new Vector2(_position.X + _offsetStepper.Offset + 42, _position.Y + 5), Color.Black);
    //        }

    //        spriteBatch.Draw(_backTexture, new Rectangle((int)(_position.X + _offsetStepper.Offset), (int)_position.Y, 42, 38), _colorStepper.Color);

    //        if (Checked || _checkSize > 0)
    //            spriteBatch.Draw(_markTexture,
    //                new Rectangle((int)(_position.X + _offsetStepper.Offset + 19 - _checkSize / 2), (int)(_position.Y + 17 - _checkSize / 2), _checkSize, _checkSize),
    //                _markColorStepper.Color);
    //    }

    //    public override void SetPosition(Vector2 position)
    //    {
    //        _position = position;
    //    }
    //}
}
