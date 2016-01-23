using Microsoft.Xna.Framework;
using Pokemon3D.GameCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    abstract class Control : GameObject
    {
        public ControlGroup Group { get; set; }
        public bool Selected { get; protected set; }

        public Control(ControlGroup group)
        {
            Group = group;
            Selected = false;
        }
        
        public virtual void Update()
        {
            if (Game.InputSystem.Mouse.HasMoved)
                if (GetBounds().Contains(Game.InputSystem.Mouse.Position))
                    Select();
        }

        public abstract void Draw();

        public abstract Rectangle GetBounds();

        public virtual void Deselect()
        {
            Selected = false;
        }

        public virtual void Select()
        {
            Selected = true;
            Group.ForEach(x =>
            {
                if (x != this)
                    x.Deselect();
            });
        }
    }
}
