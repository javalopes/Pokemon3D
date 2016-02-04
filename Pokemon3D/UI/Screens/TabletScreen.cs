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
using Pokemon3D.UI.Screens.Tablet;

namespace Pokemon3D.UI.Screens
{
    class TabletScreen : GameObject, Screen
    {
        private TextureProjectionQuad _quad;
        private Texture2D _sideTexture, _shineTexture, _backTexture, _emitterTexture, _circuitTexture;
        private int _flickerChance;
        private OffsetTransition _sideSlider;
        private ShapeRenderer _renderer;
        private int _introDelay;
        private bool _closing;
        private SpriteFont _font, _bigFont;
        private RenderTarget2D _target;
        private TabletPlugin _plugin;
        private int _pluginTitleIntro = 0;

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
            _circuitTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Circuit);
            _font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);
            _bigFont = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.BigFont);

            _sideSlider = new OffsetTransition(0f, 0.8f);
            _sideSlider.TargetOffset = 540f;

            _renderer = new ShapeRenderer(Game.SpriteBatch, Game.GraphicsDevice);

            _flickerChance = 80;
            _introDelay = 12;
            _closing = false;

            _target = new RenderTarget2D(Game.GraphicsDevice, 1200, 800);
            SetPlugin(new MainMenuPlugin());
        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.SetRenderTarget(_target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            bool flickerResult = (_flickerChance == 0 || GlobalRandomProvider.Instance.Rnd.Next(0, _flickerChance) <= 5) && _introDelay == 0;

            Game.SpriteBatch.Begin(blendState: BlendState.Additive);

            if (flickerResult)
            {
                _renderer.DrawFilledRectangle(new Rectangle((int)(_target.Width / 2 - _sideSlider.Offset), _target.Height / 2 - 280, (int)(_sideSlider.Offset * 2), 560), new Color(77, 186, 216, 230)); //new Color(87, 211, 244, 230));

                if (_sideSlider.TargetOffset == _sideSlider.Offset)
                {
                    DrawCircuit();
                }
            }

            Game.SpriteBatch.Draw(_sideTexture, new Rectangle((int)(_target.Width / 2 - 64 - _sideSlider.Offset), (int)(_target.Height / 2 - 320), 64, 640), Color.White);
            Game.SpriteBatch.Draw(_sideTexture, new Rectangle((int)(_target.Width / 2 + _sideSlider.Offset), (int)(_target.Height / 2 - 320), 64, 640), null, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);

            Game.SpriteBatch.End();

            if (flickerResult)
            {
                _plugin.Draw();
            }

            Game.GraphicsDevice.SetRenderTarget(null);

            _quad.CameraPosition = new Vector3(_cameraX, _cameraY, 1.3f);
            _quad.Texture = _target;
            var projected = _quad.GetProjected();

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(projected, Game.ScreenBounds, Color.White);
            
            Game.SpriteBatch.End();
        }
        
        private void DrawCircuit()
        {
            // TODO: give a shit

            if (!_closing)
            {
                // this extracts parts from a sprite sheet atlas and draws the repeatedly to create some kind of structure.
                // it starts at X, Y coordinates and sorta moves along them:

                int startX = 128;
                int startY = 128;

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX, startY, 16, 16), new Rectangle(0, 0, 16, 16), Color.White);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX, startY + 16, 16, 16), new Rectangle(16, 16, 16, 16), Color.White);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 16, startY + 16, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0f);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 16, startY + 32, 16, 16), new Rectangle(16, 16, 16, 16), Color.White);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 32, startY + 32, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0f);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 32, startY + 48, 16, 16), new Rectangle(16, 16, 16, 16), Color.White);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 48, startY + 48, 16, 16), new Rectangle(0, 0, 16, 16), Color.White);

                for (int i = 0; i < 7; i++)
                    Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 64 + 16 * i, startY + 48, 16, 16), new Rectangle(16, 0, 16, 16), Color.White);

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 176, startY + 48, 16, 16), new Rectangle(0, 0, 16, 16), Color.White);

                for (int i = 0; i < 7; i++)
                    Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 192 + 16 * i, startY + 48, 16, 16), new Rectangle(16, 0, 16, 16), Color.White);

                startX = 64;
                startY = 200;

                for (int i = 0; i < 4; i++)
                    Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 16 * i, startY, 16, 16), new Rectangle(16, 0, 16, 16), Color.White);

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 64, startY, 16, 16), new Rectangle(0, 0, 16, 16), Color.White);

                for (int i = 0; i < 2; i++)
                    Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 80 + 16 * i, startY, 16, 16), new Rectangle(16, 0, 16, 16), Color.White);

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 112, startY + 16, 16, 16), new Rectangle(16, 16, 16, 16), Color.White);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 112, startY, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0f);

                startX = 200;
                startY = 200;

                for (int i = 0; i < 8; i++)
                    Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 16 * i, startY, 16, 16), new Rectangle(16, 0, 16, 16), Color.White);

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 128, startY, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 128, startY - 16, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 128 + 16, startY - 16, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0f);
                Game.SpriteBatch.Draw(_circuitTexture, new Rectangle(startX + 128 + 16, startY - 32, 16, 16), new Rectangle(16, 16, 16, 16), Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);

                // draws blue lines in the background:

                _renderer.DrawLine(64, 140, 300, 140, new Color(255, 255, 255, 100));
                _renderer.DrawLine(64, 170, 400, 170, new Color(255, 255, 255, 100));
                _renderer.DrawLine(64, 200, 400, 200, new Color(255, 255, 255, 100));
                _renderer.DrawLine(200, 200, 250, 170, new Color(255, 255, 255, 100));
                _renderer.DrawLine(100, 140, 150, 170, new Color(255, 255, 255, 100));

                // draws the small subtitle:
                Game.SpriteBatch.DrawString(_font, "Holo Tablet", new Vector2(224, 182), Color.White);
                
                // draws the slowly fading in title:
                int titleLength = (int)Math.Ceiling((double)_plugin.Title.Length / 100 * _pluginTitleIntro);
                Game.SpriteBatch.DrawString(_bigFont, _plugin.Title.Substring(_plugin.Title.Length - titleLength, titleLength), new Vector2(180, 132), Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            }
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

                if (!_closing)
                {
                    UpdateTurnTablet();

                    if (_pluginTitleIntro < 100 && _sideSlider.TargetOffset == _sideSlider.Offset)
                        _pluginTitleIntro += 4;

                    if (Game.InputSystem.Dismiss(DismissInputTypes.BButton | DismissInputTypes.EscapeKey))
                    {
                        _closing = true;
                        _sideSlider.TargetOffset = 0f;
                        _sideSlider.Speed = 0.5f;
                    }
                    else
                    {
                        _plugin.Update();
                    }
                }
                else
                {
                    if (_sideSlider.Offset == _sideSlider.TargetOffset)
                    {
                        Game.ScreenManager.PopScreen();
                    }
                }
            }
        }

        private void UpdateTurnTablet()
        {
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
        }

        private void SetPlugin(TabletPlugin plugin)
        {
            _plugin = plugin;
            _pluginTitleIntro = 0;
        }

        public void OnClosing()
        {
            _target.Dispose();
        }
    }
}
