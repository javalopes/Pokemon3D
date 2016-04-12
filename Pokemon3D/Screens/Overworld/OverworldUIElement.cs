using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Screens.Overworld
{
    class OverworldUIElement
    {
        public event Action<OverworldUIElement> Draw;
        public event Action<OverworldUIElement> Update;

        public bool IsActive { get; set; }

        public OverworldScreen Screen { get; set; }

        public void OnUpdate(float elapsedTime)
        {
            Update?.Invoke(this);
        }

        public void OnDraw(GameTime gameTime)
        {
            Draw?.Invoke(this);
        }
    }
}
