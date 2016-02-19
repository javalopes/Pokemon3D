using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.UI.Framework;
using Pokemon3D.UI.Framework.Tablet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Screens.Tablet
{
    class MainMenuPlugin : TabletPlugin
    {
        private DefaultControlGroup _buttons;
        private RenderTarget2D _target;

        public MainMenuPlugin(TabletScreen screen) : base(screen)
        {
            _buttons = new DefaultControlGroup();
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(200, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Pokedex", (c) =>
            {
                screen.SetPlugin(new PokedexPlugin(screen));
            }));
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(340, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Pokemon", null));
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(480, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Inventory", null));
            _buttons.Visible = true;
            _buttons.Active = true;
            _buttons.Orientation = ControlGroupOrientation.Horizontal;

            _target = new RenderTarget2D(Game.GraphicsDevice, TabletScreen.TABLET_TARGET_WIDTH, TabletScreen.TABLET_TARGET_HEIGHT);
        }

        public override string Title
        {
            get
            {
                return "Main Menu";
            }
        }
        
        public override Texture2D Draw()
        {
            var previousTargets = Game.GraphicsDevice.GetRenderTargets();

            Game.GraphicsDevice.SetRenderTarget(_target);
            Game.GraphicsDevice.Clear(Color.Transparent);

            _buttons.Draw(samplerState: SamplerState.AnisotropicWrap);

            Game.GraphicsDevice.SetRenderTargets(previousTargets);

            return _target;
        }

        public override void Update()
        {
            _buttons.Update();
        }
    }
}
