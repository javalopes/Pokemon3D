using Microsoft.Xna.Framework;
using Pokemon3D.Common.Shapes;
using static Pokemon3D.GameCore.GameProvider;

namespace Pokemon3D.Screens.GameMenu
{
    class GameMenuScreen : Screen
    {
        private ShapeRenderer _renderer;

        public void OnOpening(object enterInformation)
        {
            _renderer = new ShapeRenderer(GameInstance.SpriteBatch);
        }

        public void OnEarlyDraw(GameTime gameTime)
        { }

        public void OnLateDraw(GameTime gameTime)
        {
            GameInstance.SpriteBatch.Begin();

            _renderer.DrawShapeGradientFill(new Plane2D(0, 0, 100, 100), null, Color.White, Color.Black, true);

            GameInstance.SpriteBatch.End();
        }

        public void OnUpdate(GameTime gameTime)
        {

        }

        public void OnClosing()
        {

        }
    }
}
