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

        public void OnOpening(object enterInformation)
        {
            _quad = new TextureProjectionQuad();
            _quad.TextureOutputWidth = 544 * 2;
            _quad.TextureOutputHeight = 320 * 2;
            
            _sideTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Side);
            _shineTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Shine);
            _backTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Back);
            _emitterTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Tablet_Emitter);

            _sideSlider = new OffsetTransition(0f, 0.8f);
            _sideSlider.TargetOffset = 240f;

            _renderer = new ShapeRenderer(Game.SpriteBatch, Game.GraphicsDevice);

            _flickerChance = 80;
        }

        public void OnDraw(GameTime gameTime)
        {
            RenderTarget2D target = new RenderTarget2D(Game.GraphicsDevice, 544, 320);
            Game.GraphicsDevice.SetRenderTarget(target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            Game.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            if (_flickerChance == 0 || GlobalRandomProvider.Instance.Rnd.Next(0, _flickerChance) <= 5)
            {
                _renderer.DrawFilledRectangle(new Rectangle((int)(target.Width / 2 - _sideSlider.Offset), target.Height / 2 - 130, (int)(_sideSlider.Offset * 2), 260), new Color(87, 211, 244, 210));
            }

            Game.SpriteBatch.Draw(_sideTexture, new Vector2(target.Width / 2 - 32 - _sideSlider.Offset, target.Height / 2 - 160), Color.White);
            Game.SpriteBatch.Draw(_sideTexture, new Vector2(target.Width / 2 + _sideSlider.Offset, target.Height / 2 - 160), color: Color.White, effects: SpriteEffects.FlipHorizontally);
            
            Game.SpriteBatch.End();

            Game.GraphicsDevice.SetRenderTarget(null);

            _quad.CameraPosition = new Vector3(0, -0.3f, 1.3f);
            _quad.Texture = target;
            var projected = _quad.GetProjected();

            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(projected, Game.ScreenBounds, Color.White);
            
            Game.SpriteBatch.End();

            target.Dispose();
        }

        public void OnUpdate(float elapsedTime)
        {
            if (_flickerChance > 0)
                _flickerChance--;

            _sideSlider.Update();
            if (Game.InputSystem.Dismiss(DismissInputTypes.BButton | DismissInputTypes.EscapeKey))
            {
                Game.ScreenManager.PopScreen();
            }
        }

        public void OnClosing()
        {

        }
    }
}
