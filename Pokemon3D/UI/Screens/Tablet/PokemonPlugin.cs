using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pokemon3D.UI.Framework;
using Pokemon3D.UI.Framework.Tablet;

namespace Pokemon3D.UI.Screens.Tablet
{
    class PokemonPlugin : TabletPlugin
    {
        private RenderTarget2D _target;
        private DefaultControlGroup _buttons;
        private SpriteBatch _batch;

        public override string Title
        {
            get { return "Pokemon"; }
        }

        public PokemonPlugin(TabletScreen screen) : base(screen)
        {
            _buttons = new DefaultControlGroup();
            for (int i = 0; i < Game.LoadedSave.PartyPokemon.Count; i++)
            {
                _buttons.Add(new PokemonProfile(Game.ActiveGameMode, Game.LoadedSave.PartyPokemon[i], new Vector2(100 + i * 160, 260)));
            }
            
            _buttons.Visible = true;
            _buttons.Active = true;

            _batch = new SpriteBatch(Game.GraphicsDevice);

            _target = new RenderTarget2D(Game.GraphicsDevice, TabletScreen.TABLET_TARGET_WIDTH, TabletScreen.TABLET_TARGET_HEIGHT);
        }

        public override Texture2D Draw()
        {
            var previousTargets = Game.GraphicsDevice.GetRenderTargets();

            Game.GraphicsDevice.SetRenderTarget(_target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            _batch.Begin(blendState: BlendState.Additive);

            _buttons.Draw(blendState: BlendState.NonPremultiplied);

            _batch.End();

            Game.GraphicsDevice.SetRenderTargets(previousTargets);

            return _target;
        }

        public override void Update()
        {
            _buttons.Update();
        }
    }
}
