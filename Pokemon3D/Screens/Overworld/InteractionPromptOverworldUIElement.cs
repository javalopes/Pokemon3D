using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Shapes;
using Pokemon3D.Entities.System;

namespace Pokemon3D.Screens.Overworld
{
    class InteractionPromptOverworldUIElement : OverworldUIElement
    {
        private Entity _owner;

        private float _buttonPressed = 0f;
        private Pie2D _buttonChart = null;

        public string Message { get; set; }
        public event Action<InteractionPromptOverworldUIElement> InteractionStarted;

        public InteractionPromptOverworldUIElement(Entity owner, string message)
            : base()
        {
            _owner = owner;
            Message = message;

            _buttonChart = new Pie2D(Game.GraphicsDevice, 24, 0f, 20, Vector2.Zero, false);
            _buttonChart.PrimaryColor = Color.Gray;
            _buttonChart.SecondaryColor = Color.Black;
            _buttonChart.ChartType = PieChartType.RadialFill;
        }

        public override void Draw(GameTime gameTime)
        {
            var overworldScreen = Screen;
            var player = overworldScreen.ActiveWorld.Player;
            var camera = player.Camera;

            var position = camera.Viewport.Project(_owner.Position + new Vector3(0, 0.5f, 0), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            var position2D = new Vector2(position.X, position.Y);

            var font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
            var fontSize = font.MeasureString(Message);

            Game.ShapeRenderer.DrawRectangle(new Rectangle((int)(position2D.X - fontSize.X / 2f - 3), (int)(position2D.Y - 1), (int)fontSize.X + 6, 30), new Color(0, 0, 0, 180));

            Game.SpriteBatch.DrawString(font, Message, position2D + new Vector2(-(fontSize.X / 2f), 0), Color.White);

            Game.ShapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24, (int)position2D.Y + 28, 48, 48), new Color(0, 0, 0, 180));

            _buttonChart.Position = position2D + new Vector2(-24, 28);
            _buttonChart.DrawBatched(Game.SpriteBatch);

            Game.ShapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24 + 8, (int)position2D.Y + 28 + 8, 32, 32), new Color(64, 200, 64));
            Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A), new Rectangle((int)position2D.X - 24 + 8, (int)position2D.Y + 28 + 8, 32, 32), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Game.InputSystem.GamePad.IsButtonDown(Buttons.A))
            {
                _buttonPressed += 0.035f;
                if (_buttonPressed >= 1f)
                {
                    _buttonPressed = 1f;
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
            base.ActiveStateChanged();

            if (!IsActive)
                _buttonPressed = 0f;
        }
    }
}
