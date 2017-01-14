using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameProvider;

namespace Pokemon3D.UI
{
    class ColoredEllipse : UiElement
    {
        public ColoredEllipse(Color color, Ellipse ellipse)
        {
            Bounds = ellipse.Bounds;
            Color = color;
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GetBounds();
            GameInstance.GetService<ShapeRenderer>().DrawEllipse(new Ellipse(bounds.X, bounds.Y, bounds.Width, Bounds.Height), Color);
        }
    }
}
