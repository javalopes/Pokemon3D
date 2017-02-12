using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;

namespace Pokemon3D.UI
{
    class ColoredPie : UiElement
    {
        public ColoredPie(Color color, Ellipse ellipse)
        {
            Bounds = ellipse.Bounds;
            Color = color;
            Angle = MathHelper.TwoPi;
        }

        public float Angle { get; set; }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GetBounds();
            GameProvider.IGameInstance.GetService<ShapeRenderer>().DrawEllipsePie(new Ellipse(bounds.X, bounds.Y, bounds.Width, Bounds.Height), Color, Angle);
        }
    }
}