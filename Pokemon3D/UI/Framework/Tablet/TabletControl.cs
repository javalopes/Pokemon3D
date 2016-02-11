using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework.Tablet
{
    abstract class TabletControl : Control
    {
        protected TextureProjectionQuad _quad;

        public TabletControl(TextureProjectionQuad quad)
        {
            _quad = quad;
        }
    }
}
