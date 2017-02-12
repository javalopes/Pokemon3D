using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Shapes;
using Pokemon3D.Rendering.UI;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.UI
{
    internal class ColoredRectangle : UiElement
    {
        public ColoredRectangle(Color color, Rectangle bounds)
        {
            Bounds = bounds;
            Color = color;
        }

        public override bool IsInteractable => false;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var bounds = GetBounds();

            IGameInstance.GetService<ShapeRenderer>().DrawRectangle(bounds, Color);
        }
    }
}
