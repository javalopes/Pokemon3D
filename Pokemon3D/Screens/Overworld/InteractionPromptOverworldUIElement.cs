using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Content;
using Pokemon3D.Entities.System;
using Pokemon3D.GameCore;
using Pokemon3D.ScriptPipeline;
using static GameProvider;

namespace Pokemon3D.Screens.Overworld
{
    class InteractionPromptOverworldUIElement : MessageOverworldUIElement
    {
        private readonly Entity _owner;

        private float _buttonPressed = 0f;
        private readonly Pie2D _buttonChart = null;

        public string Message { get; set; }
        public event Action<InteractionPromptOverworldUIElement> InteractionStarted;

        public InteractionPromptOverworldUIElement(Entity owner, string message) : base(message)
        {
            _owner = owner;
            Message = message;

            _buttonChart = new Pie2D(GameInstance.GraphicsDevice, 24, 0f, 20, Vector2.Zero, false)
            {
                PrimaryColor = Color.LightGray,
                SecondaryColor = Color.Black,
                ChartType = PieChartType.RadialFill
            };
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Screen.HasBlockingElements &&
                ScriptPipelineManager.ActiveProcessorCount == 0)
            {
                var player = Screen.ActiveWorld.Player;
                var camera = player.Camera;
                var spriteBatch = GameInstance.GetService<SpriteBatch>();
                var shapeRenderer = GameInstance.GetService<ShapeRenderer>();

                var position = camera.Viewport.Project(_owner.Position + new Vector3(0, 0.5f, 0), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
                var position2D = new Vector2(position.X, position.Y);

                var font = GameInstance.Content.Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
                var fontSize = font.MeasureString(Message);

                shapeRenderer.DrawRectangle(new Rectangle((int)(position2D.X - fontSize.X / 2f - 3), (int)(position2D.Y - 1), (int)fontSize.X + 6, 30), new Color(0, 0, 0, 180));

                spriteBatch.DrawString(font, Message, position2D + new Vector2(-(fontSize.X / 2f), 0), Color.White);

                shapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24, (int)position2D.Y + 28, 48, 48), new Color(0, 0, 0, 180));

                _buttonChart.Position = position2D + new Vector2(-24, 28);
                _buttonChart.DrawBatched(spriteBatch);

                shapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24 + 8, (int)position2D.Y + 28 + 8, 32, 32), new Color(64, 200, 64));
                spriteBatch.Draw(GameInstance.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A), new Rectangle((int)position2D.X - 24 + 8, (int)position2D.Y + 28 + 8, 32, 32), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GameInstance.GetService<InputSystem>().IsPressedOnce(ActionNames.MenuAccept) &&
                !Screen.HasBlockingElements &&
                ScriptPipelineManager.ActiveProcessorCount == 0) // only update these if this are no scripts running.
            {
                _buttonPressed += 0.035f;
                if (_buttonPressed >= 1f)
                {
                    _buttonPressed = 0f;
                    InteractionStarted?.Invoke(this);
                }
            }
            else
            {
                if (_buttonPressed > 0f)
                {
                    _buttonPressed -= 0.1f;
                    if (_buttonPressed <= 0f)
                    {
                        _buttonPressed = 0f;
                    }
                }
            }

            _buttonChart.Angle = MathHelper.TwoPi * _buttonPressed;
        }

        protected override void ActiveStateChanged()
        {
            if (!IsActive)
                _buttonPressed = 0f;
        }
    }
}
