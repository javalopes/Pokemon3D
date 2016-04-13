using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Screens.Overworld
{
    abstract class OverworldUIElement
    {
        private bool _isActive = false;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                ActiveStateChanged();
            }
        }

        public OverworldScreen Screen { get; set; }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        protected virtual void ActiveStateChanged() { }
    }
}
