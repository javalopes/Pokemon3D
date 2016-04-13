using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;

namespace Pokemon3D.Screens.Overworld
{
    abstract class OverworldUIElement : GameObject
    {
        public bool IsActive { get; set; }

        public OverworldScreen Screen { get; set; }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
