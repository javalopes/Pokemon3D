using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Screens.Tablet
{
    abstract class TabletPlugin : GameObject
    {
        public TabletScreen ActiveTabletScreen
        {
            get { return (TabletScreen)Game.ScreenManager.CurrentScreen; }
        }
        
        public abstract void Draw();

        public abstract void Update();

        public abstract string Title { get; }
    }
}
