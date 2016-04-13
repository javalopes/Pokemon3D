using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Represents a control that the user can interact with on a screen.
    /// </summary>
    abstract class Control
    {
        /// <summary>
        /// The <see cref="ControlGroup"/> this control belongs to.
        /// </summary>
        public ControlGroup Group { get; set; }
        /// <summary>
        /// If this control is selected inside its control group.
        /// </summary>
        public bool Selected { get; protected set; }

        protected Control()
        {
            Selected = false;
        }

        public virtual void Update()
        {
            if (GameInstance.InputSystem.Mouse.HasMoved)
                if (GetBounds().Contains(GameInstance.InputSystem.Mouse.Position))
                    Select();
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract Rectangle GetBounds();

        public abstract void SetPosition(Vector2 position);

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
            Group.Active = true;
        }

        public virtual void GroupActivated() { }
        public virtual void GroupDeactivated() { }
    }
}
