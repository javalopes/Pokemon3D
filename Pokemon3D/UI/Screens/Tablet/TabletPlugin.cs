using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.UI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Screens.Tablet
{
    abstract class TabletPlugin : GameObject
    {
        private TabletScreen _screen;

        public TabletPlugin(TabletScreen screen)
        {
            _screen = screen;
        }

        public TabletScreen ActiveTabletScreen
        {
            get { return _screen; }
        }
        
        public TextureProjectionQuad ActiveQuad
        {
            get { return ActiveTabletScreen.ActiveQuad; }
        }

        public abstract Texture2D Draw();

        public abstract void Update();

        public abstract string Title { get; }
    }
}
