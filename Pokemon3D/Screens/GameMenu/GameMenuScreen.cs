using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Shapes;
using static GameProvider;

namespace Pokemon3D.Screens.GameMenu
{
    internal class GameMenuScreen : Screen
    {
        public void OnOpening(object enterInformation)
        {
        }

        public void OnEarlyDraw(GameTime gameTime)
        { }

        public void OnLateDraw(GameTime gameTime)
        {
            var spriteBatch = GameInstance.GetService<SpriteBatch>();

            spriteBatch.Begin();
            GameInstance.GetService<ShapeRenderer>().DrawShapeGradientFill(new Plane2D(0, 0, 100, 100), null, Color.White, Color.Black, true);
            spriteBatch.End();
        }

        public void OnUpdate(GameTime gameTime)
        {

        }

        public void OnClosing()
        {

        }
    }
}
