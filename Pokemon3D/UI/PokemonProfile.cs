namespace Pokemon3D.UI
{
    //class PokemonProfile : UiElement
    //{
    //    private Vector2 _position;
    //    private readonly Texture2D _profileHPIndicatorTexture;
    //    private readonly Texture2D _profileBackTexture;
    //    private readonly Pokemon _pokemon;
    //    private readonly PokemonSpriteSheet _sheet;

    //    private const int PROFILE_WIDTH = 120;
    //    private const int PROFILE_HEIGHT = 105;
    //    private const int HP_INDICATOR_OFFSET = 4;

    //    private readonly OffsetTransition _HPIndicatorStepper;
    //    private readonly Color _HPindicatorColor;

    //    private readonly ColorTransition _colorStepper;
    //    private readonly OffsetTransition _expandStepper;

    //    public PokemonProfile(GameMode gameMode, Pokemon pokemon, Vector2 position) : base(null, null)
    //    {
    //        _pokemon = pokemon;
    //        _position = position;

    //        _profileHPIndicatorTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Windows.Textures.UI.Common.Profile);
    //        _profileBackTexture = GameInstance.Content.Load<Texture2D>(ResourceNames.Windows.Textures.UI.Common.Profile_Shadow);

    //        var dataModel = _pokemon.ActiveFormModel.FrontSpriteSheet;
    //        _sheet = new PokemonSpriteSheet(gameMode.GetTexture(dataModel.Source), dataModel.FrameSize.Width, dataModel.FrameSize.Height);

    //        var pokemonHpValue = (double)_pokemon.HP / _pokemon.MaxHP;
    //        _HPIndicatorStepper = new OffsetTransition(0f, 0.7f) {TargetOffset = GetHPIndicatorHeight(pokemonHpValue)};
    //        _HPindicatorColor = GetHPIndicatorColor(pokemonHpValue);

    //        _colorStepper = new ColorTransition(new Color(255, 255, 255), 0.5f);
    //        _expandStepper = new OffsetTransition(0f, 0.5f);
    //    }

    //    public override void OnAction()
    //    {
    //    }

    //    public override void Draw(SpriteBatch spriteBatch)
    //    {
    //        spriteBatch.Draw(_profileBackTexture,
    //            new Rectangle((int)(_position.X - _expandStepper.Offset / 2),
    //                          (int)(_position.Y - _expandStepper.Offset / 2),
    //                          (int)(_profileBackTexture.Width + _expandStepper.Offset),
    //                          (int)(_profileBackTexture.Height + _expandStepper.Offset)),
    //            _colorStepper.Color);

    //        if (_pokemon.HP > 0)
    //        {
    //            // calculate the height of the hp indicator texture:
    //            var height = (int)_HPIndicatorStepper.Offset;

    //            spriteBatch.Draw(_profileHPIndicatorTexture,
    //                new Rectangle((int)_position.X + HP_INDICATOR_OFFSET, (int)_position.Y + HP_INDICATOR_OFFSET + (PROFILE_HEIGHT - height), PROFILE_WIDTH, height),
    //                new Rectangle(0, PROFILE_HEIGHT - height, PROFILE_WIDTH, height),
    //                _HPindicatorColor);
    //        }

    //        int pokemonWidth = _sheet.CurrentFrame.Width;
    //        int pokemonHeight = _sheet.CurrentFrame.Height;

    //        spriteBatch.Draw(_sheet.CurrentFrame,
    //            new Rectangle((int)(_position.X + HP_INDICATOR_OFFSET + PROFILE_WIDTH / 2 - pokemonWidth / 2),
    //            (int)(_position.Y + HP_INDICATOR_OFFSET + PROFILE_HEIGHT / 2 - pokemonHeight / 2), pokemonWidth, pokemonHeight), Color.White);
    //    }

    //    private int GetHPIndicatorHeight(double HPValue)
    //    {
    //        return (int)Math.Ceiling(105 * HPValue);
    //    }

    //    private Color GetHPIndicatorColor(double HPValue)
    //    {
    //        if (HPValue >= 0.5) return new Color(0, 193, 111);
    //        if (HPValue < 0.5 && HPValue > 0.2) return new Color(255, 213, 0);
    //            return new Color(201, 45, 0);
    //        }
    //    }

    //    public override Rectangle GetBounds()
    //    {
    //        return new Rectangle((int)_position.X + 4, (int)_position.Y + 4, PROFILE_WIDTH, PROFILE_HEIGHT);
    //    }

    //    public override void SetPosition(Vector2 position)
    //    {
    //        _position = position;
    //    }

    //    public override void Select()
    //    {
    //        base.Select();

    //        TriggerSelected();
    //    }

    //    public override void Deselect()
    //    {
    //        base.Deselect();

    //        TriggerDeselected();
    //    }

    //    public override void GroupActivated()
    //    {
    //        if (Selected)
    //            TriggerSelected();
    //    }

    //    public override void GroupDeactivated()
    //    {
    //        TriggerDeselected();
    //    }

    //    private void TriggerSelected()
    //    {
    //        _colorStepper.TargetColor = new Color(100, 193, 238);
    //        _expandStepper.TargetOffset = 30f;
    //    }

    //    private void TriggerDeselected()
    //    {
    //        _colorStepper.TargetColor = new Color(255, 255, 255);
    //        _expandStepper.TargetOffset = 0f;
    //    }

    //    public override void Update()
    //    {
    //        base.Update();

    //        _sheet.Update();

    //        _HPIndicatorStepper.Update();
    //        _colorStepper.Update();
    //        _expandStepper.Update();
    //    }
    //}
}
