using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Framework;
using Pokemon3D.GameModes.Pokemon;

namespace Pokemon3D.UI.Screens
{
    class PokemonTableScreen : GameObject, Screen
    {
        private Card[] _cards;

        public void OnOpening(object enterInformation)
        {
            var party = Game.LoadedSave.PartyPokemon;
            _cards = new Card[party.Count];
            for (int i = 0; i < party.Count; i++)
                _cards[i] = new Card(party[i]);
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();

            for (int i = 0; i < 4; i++)
            {
                var card = _cards[i];
                Game.SpriteBatch.Draw(card.GetTexture(), new Vector2(100 + i * 240, 100), Color.White);
            }

            Game.SpriteBatch.Draw(_cards[4].GetTexture(), new Rectangle(Game.ScreenBounds.Width - 100, 200, 110, 160), null, Color.White, MathHelper.PiOver2, new Vector2(110, 160), SpriteEffects.None, 0f);
            Game.SpriteBatch.Draw(_cards[2].GetTexture(), new Rectangle(Game.ScreenBounds.Width - 100, 280, 110, 160), null, Color.White, MathHelper.PiOver2, new Vector2(110, 160), SpriteEffects.None, 0f);

            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {
            for (int i = 0; i < _cards.Length; i++)
            {
                var card = _cards[i];
                card.Update();
            }
        }

        public void OnClosing()
        {

        }

        private class Card : GameObject
        {
            private static Texture2D _cardBack, _hexagons, _flair, _HPIndicator;
            private static SpriteFont _bigFont, _normalFont;
            private PokemonSpriteSheet _sheet;
            private Pokemon _pokemon;
            private Color _backColor, _frontColor;
            private SpriteBatch _batch;
            private RenderTarget2D _target;
            private ColorTransition _alphaStepper;

            private const int WIDTH = 220,
                              HEIGHT = 320;

            public Card(Pokemon pokemon)
            {
                if (_cardBack == null)
                {
                    _cardBack = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Cards.CardBack);
                    _hexagons = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Cards.CardTypeBack);
                    _flair = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Cards.CardTypeFront);
                    _bigFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);
                    _normalFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);
                    _HPIndicator = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Cards.CardHP);
                }

                _pokemon = pokemon;
                var dataModel = _pokemon.ActiveFormModel.FrontSpriteSheet;
                _sheet = new PokemonSpriteSheet(Game.ActiveGameMode.GetTexture(dataModel.Source), dataModel.FrameSize.Width, dataModel.FrameSize.Height);

                _frontColor = pokemon.Type1.Color.GetColor();
                if (pokemon.Type2 == null)
                    _backColor = _frontColor;
                else
                    _backColor = pokemon.Type2.Color.GetColor();

                _alphaStepper = new ColorTransition(new Color(_frontColor.R, _frontColor.G, _frontColor.B, 255), 0.9f);

                _target = new RenderTarget2D(Game.GraphicsDevice, WIDTH, HEIGHT);
                _batch = new SpriteBatch(Game.GraphicsDevice);
            }

            public Texture2D GetTexture()
            {
                var previousTargets = Game.GraphicsDevice.GetRenderTargets();
                Game.GraphicsDevice.SetRenderTarget(_target);
                Game.GraphicsDevice.Clear(Color.Transparent);

                _batch.Begin(blendState: BlendState.AlphaBlend);

                _batch.Draw(_cardBack, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White);
                _batch.Draw(_hexagons, new Rectangle(0, 0, WIDTH, HEIGHT), new Color(_backColor.R, _backColor.G, _backColor.B, _alphaStepper.Color.A));

                var frame = _sheet.CurrentFrame;

                int frameWidth = frame.Width;
                int frameHeight = frame.Height;
                if (frameHeight > 128)
                {
                    float diff = 128f / frameHeight;
                    frameHeight = (int)(frameHeight * diff);
                    frameWidth = (int)(frameWidth * diff);
                    _batch.Draw(frame, new Rectangle((int)(WIDTH / 2f - frame.Width / 2f), 22,
                                                     frameWidth, frameHeight), Color.White);
                }
                else
                {
                    _batch.Draw(frame, new Rectangle((int)(WIDTH / 2f - frame.Width / 2f), (int)(86 - frame.Height / 2f), frame.Width, frame.Height), Color.White);
                }

                _batch.Draw(_flair, new Rectangle(0, 0, WIDTH, HEIGHT), _alphaStepper.Color);

                var textSize = _bigFont.MeasureString(_pokemon.DisplayName) * 0.75f;
                _batch.DrawString(_bigFont, _pokemon.DisplayName, new Vector2(WIDTH / 2 - textSize.X / 2, 140), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

                _batch.DrawString(_bigFont, "Lv. " + _pokemon.Level, new Vector2(14, 151), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

                int hpWidth = (int)(_HPIndicator.Width * (_pokemon.HP / (float)_pokemon.MaxHP));
                if (hpWidth < 6 && _pokemon.HP > 0)
                    hpWidth = 6;
                else if (_pokemon.HP == _pokemon.MaxHP)
                    hpWidth = _HPIndicator.Width;
                else if (_pokemon.HP == 0)
                    hpWidth = 0;

                _batch.Draw(_HPIndicator, new Rectangle(67, 155, hpWidth, _HPIndicator.Height), new Rectangle(0, 0, hpWidth, _HPIndicator.Height), new Color(255, 255, 255, _alphaStepper.Color.A));

                _batch.End();

                Game.GraphicsDevice.SetRenderTargets(previousTargets);

                return _target;
            }

            public void Update()
            {
                _sheet.Update();
                _alphaStepper.Update();
                if (_alphaStepper.Finished)
                {
                    if (_alphaStepper.Color.A == 255)
                        _alphaStepper.TargetColor = new Color(_frontColor.R, _frontColor.G, _frontColor.B, 0);
                    else
                        _alphaStepper.TargetColor = new Color(_frontColor.R, _frontColor.G, _frontColor.B, 255);
                }
            }
        }
    }
}
