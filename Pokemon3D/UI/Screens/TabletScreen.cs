using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Framework;
using Pokemon3D.Common;
using Pokemon3D.Common.Input;

namespace Pokemon3D.UI.Screens
{
    class TabletScreen : GameObject, Screen
    {
        private TextureProjectionQuad _quad;
        private Texture2D _sideTexture, _shineTexture, _backTexture, _emitterTexture;
        private int _flickerChance = 100;
        private OffsetTransition _sideSlider;
        private ShapeRenderer _renderer;
        private int _introDelay = 20;

        private DefaultControlGroup _pokemonProfiles;

        private float _cameraY = -0.3f;
        private float _cameraX = 0.0f;

        public void OnOpening(object enterInformation)
        {
            _quad = new TextureProjectionQuad();
            _quad.TextureOutputWidth = 1200;
            _quad.TextureOutputHeight = 800;

            _sideTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Side);
            _shineTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Shine);
            _backTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Back);
            _emitterTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Emitter);

            _sideSlider = new OffsetTransition(0f, 0.8f);
            _sideSlider.TargetOffset = 540f;

            _renderer = new ShapeRenderer(Game.SpriteBatch, Game.GraphicsDevice);

            _flickerChance = 80;
            _introDelay = 20;

            // profiles initialization:
            _pokemonProfiles = new DefaultControlGroup();

            for (int i = 0; i < Game.LoadedSave.PartyPokemon.Count; i++)
            {
                var pokemon = Game.LoadedSave.PartyPokemon[i];
                _pokemonProfiles.Add(new PokemonProfile(Game.ActiveGameMode, pokemon, new Vector2(110 * i + 340, 150 + ((i % 2) * 52))));
            }

            _pokemonProfiles.Active = false;
            _pokemonProfiles.Visible = true;
            _pokemonProfiles.Orientation = ControlGroupOrientation.Horizontal;
        }

        public void OnDraw(GameTime gameTime)
        {
            RenderTarget2D target = new RenderTarget2D(Game.GraphicsDevice, 1200, 800);
            Game.GraphicsDevice.SetRenderTarget(target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            bool flickerResult = (_flickerChance == 0 || GlobalRandomProvider.Instance.Rnd.Next(0, _flickerChance) <= 5) && _introDelay == 0;

            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            if (flickerResult)
            {
                _renderer.DrawFilledRectangle(new Rectangle((int)(target.Width / 2 - _sideSlider.Offset), target.Height / 2 - 280, (int)(_sideSlider.Offset * 2), 560), new Color(87, 211, 244, 210));
            }

            Game.SpriteBatch.Draw(_sideTexture, new Rectangle((int)(target.Width / 2 - 32 - _sideSlider.Offset), (int)(target.Height / 2 - 320), 64, 640), Color.White);
            Game.SpriteBatch.Draw(_sideTexture, new Rectangle((int)(target.Width / 2 + _sideSlider.Offset), (int)(target.Height / 2 - 320), 64, 640), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            Game.SpriteBatch.End();

            if (flickerResult)
            {
                _pokemonProfiles.Draw(blendState: BlendState.NonPremultiplied);
            }

            Game.GraphicsDevice.SetRenderTarget(null);

            _quad.CameraPosition = new Vector3(_cameraX, _cameraY, 1.3f);
            _quad.Texture = target;
            var projected = _quad.GetProjected();

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(projected, Game.ScreenBounds, Color.White);
            
            Game.SpriteBatch.End();

            target.Dispose();
        }

        public void OnUpdate(float elapsedTime)
        {
            if (_introDelay > 0)
            {
                _introDelay--;
            }
            else
            {
                if (_flickerChance > 0)
                    _flickerChance--;

                _sideSlider.Update();
                _pokemonProfiles.Update();

                bool hasInput = false;

                if (Game.InputSystem.Left(InputDetectionType.HeldDown, DirectionalInputTypes.RightThumbstick))
                {
                    if (_cameraX < 0.6f)
                        _cameraX += 0.02f;
                    hasInput = true;
                }
                if (Game.InputSystem.Right(InputDetectionType.HeldDown, DirectionalInputTypes.RightThumbstick))
                {
                    if (_cameraX > -0.6f)
                        _cameraX -= 0.02f;
                    hasInput = true;
                }
                if (Game.InputSystem.Down(InputDetectionType.HeldDown, DirectionalInputTypes.RightThumbstick))
                {
                    if (_cameraY < 0.6f)
                        _cameraY += 0.02f;
                    hasInput = true;
                }
                if (Game.InputSystem.Up(InputDetectionType.HeldDown, DirectionalInputTypes.RightThumbstick))
                {
                    if (_cameraY > -1.2f)
                        _cameraY -= 0.02f;
                    hasInput = true;
                }
                if (!hasInput)
                {
                    if (_cameraY < -0.3f)
                    {
                        _cameraY += 0.1f;
                        if (_cameraY >= -0.3f)
                            _cameraY = -0.3f;
                    }
                    else if (_cameraY > -0.3f)
                    {
                        _cameraY -= 0.1f;
                        if (_cameraY <= -0.3f)
                            _cameraY = -0.3f;
                    }

                    if (_cameraX < -0.0f)
                    {
                        _cameraX += 0.1f;
                        if (_cameraX >= 0.0f)
                            _cameraX = 0.0f;
                    }
                    else if (_cameraX > -0.0f)
                    {
                        _cameraX -= 0.1f;
                        if (_cameraX <= 0.0f)
                            _cameraX = 0.0f;
                    }
                }

                if (Game.InputSystem.Dismiss(DismissInputTypes.BButton | DismissInputTypes.EscapeKey))
                {
                    Game.ScreenManager.PopScreen();
                }
            }
        }

        public void OnClosing()
        {

        }
    }
}
