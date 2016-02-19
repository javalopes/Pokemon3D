using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Screens.Tablet
{
    class PokedexPlugin : TabletPlugin
    {
        private Texture2D _loadContainerTexture;
        private Texture2D[] _noise;
        private SpriteBatch _batch;
        private int _noiseIndex = 0;
        private RenderTarget2D _target;

        public override string Title
        {
            get { return "Pokedex"; }
        }

        public PokedexPlugin(TabletScreen screen) : base(screen)
        {
            _loadContainerTexture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Tablet.Pokedex.LoadContainer);
            _batch = new SpriteBatch(Game.GraphicsDevice);

            _noise = new Texture2D[7];
            for (int i = 0; i < _noise.Length; i++)
                _noise[i] = GenerateNoiseTexture();

            _target = new RenderTarget2D(Game.GraphicsDevice, TabletScreen.TABLET_TARGET_WIDTH, TabletScreen.TABLET_TARGET_HEIGHT);
        }

        private Texture2D GenerateNoiseTexture()
        {
            var texture = new Texture2D(Game.GraphicsDevice, 180, 120);
            var colorArr = new Color[texture.Width * texture.Height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    float maxChance = 0.5f - (Math.Abs((texture.Height / 2f - y) / (texture.Height / 2f)) / 2f) + 
                                      0.5f - (Math.Abs((texture.Width / 2f - x) / (texture.Width / 2f)) / 2f);

                    int highEnd = (int)Math.Ceiling(255f * maxChance) - 128;
                    int index = y * texture.Width + x;

                    if (highEnd > 0)
                        colorArr[index] = new Color(255, 255, 255, Common.GlobalRandomProvider.Instance.Rnd.Next(0, highEnd));

                }
            }

            texture.SetData(colorArr);
            return texture;
        }

        public override Texture2D Draw()
        {
            var previousTargets = Game.GraphicsDevice.GetRenderTargets();
            
            Game.GraphicsDevice.SetRenderTarget(_target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            _batch.Begin(blendState: BlendState.Additive);
            
            var noiseTexture = _noise[_noiseIndex];

            _batch.Draw(noiseTexture,
                new Rectangle((int)(TabletScreen.TABLET_TARGET_WIDTH / 2f - (_loadContainerTexture.Width + 200) / 2f),
                (int)(TabletScreen.TABLET_TARGET_HEIGHT / 2f - (_loadContainerTexture.Height + 100) / 2f),
                _loadContainerTexture.Width + 200,
                _loadContainerTexture.Height + 100),
                Color.White);

            _batch.Draw(_loadContainerTexture,
                new Vector2(TabletScreen.TABLET_TARGET_WIDTH / 2 - _loadContainerTexture.Width / 2,
                TabletScreen.TABLET_TARGET_HEIGHT / 2 - _loadContainerTexture.Height / 2),
                Color.White);

            _batch.End();

            Game.GraphicsDevice.SetRenderTargets(previousTargets);

            return _target;
        }

        public override void Update()
        {
            _noiseIndex++;
            if (_noiseIndex == _noise.Length)
                _noiseIndex = 0;
        }
    }
}
