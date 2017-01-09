using Pokemon3D.Rendering.UI;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Rendering.Compositor;
using Microsoft.Xna.Framework;
using static Pokemon3D.GameProvider;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Entities;
using Pokemon3D.Content;

namespace Pokemon3D.UI
{
    class RenderStatisticsView : UiElement
    {
        private SpriteFont _debugSpriteFont;
        private ShapeRenderer _shapeRenderer;
        private World _world;

        public override bool IsInteractable => false;

        public RenderStatisticsView(World activeWorld)
        {
            _world = activeWorld;
            _shapeRenderer = GameInstance.GetService<ShapeRenderer>();
            _debugSpriteFont = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.DebugFont);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var renderStatistics = RenderStatistics.Instance;

            const int spacing = 5;
            var elementHeight = _debugSpriteFont.LineSpacing + spacing;
            var height = elementHeight * 4 + spacing;
            const int width = 180;

            var startPosition = new Vector2(0, GameInstance.ScreenBounds.Height - height);

            _shapeRenderer.DrawRectangle((int)startPosition.X,
                (int)startPosition.Y,
                width,
                height,
                Color.DarkGreen);

            startPosition.X += spacing;
            startPosition.Y += spacing;
            spriteBatch.DrawString(_debugSpriteFont, $"Average DrawTime[ms]: {renderStatistics.AverageDrawTime:0.00}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            spriteBatch.DrawString(_debugSpriteFont, $"Total Drawcalls: {renderStatistics.DrawCalls}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            spriteBatch.DrawString(_debugSpriteFont, $"Entity Count: {_world.EntitySystem.EntityCount}", startPosition, Color.White);
            startPosition.Y += elementHeight;
            spriteBatch.DrawString(_debugSpriteFont, $"Mesh Instances: {Rendering.Data.Mesh.InstanceCount}", startPosition, Color.White);
        }
    }
}
