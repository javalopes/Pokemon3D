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

        public MainMenuPlugin(TabletScreen screen) : base(screen)
        {
            _buttons = new DefaultControlGroup();
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(200, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Pokedex", null));
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(340, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Pokemon", null));
            _buttons.Add(new MainMenuButton(ActiveQuad, new Vector2(480, 250), ResourceNames.Textures.UI.Tablet.MainMenu.Pokeball, "Inventory", null));
            _buttons.Visible = true;
            _buttons.Active = true;
            _buttons.Orientation = ControlGroupOrientation.Horizontal;
        }

        public override string Title
        {
            get
            {
                return "Main Menu";
            }
        }
        
        public override void Draw()
        {
            _buttons.Draw(samplerState: SamplerState.AnisotropicWrap);
        }

        public override void Update()
        {
            _buttons.Update();
        }
    }
}
