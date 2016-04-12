using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Collisions;
using Pokemon3D.Screens.Overworld;
using Microsoft.Xna.Framework;
using Pokemon3D.Rendering;
using Pokemon3D.Common.Shapes;
using Microsoft.Xna.Framework.Graphics;

namespace Pokemon3D.Entities.System.Components
{
    [JsonComponentId("scripttrigger")]
    class ScriptTriggerEntityComponent : EntityComponent
    {
        private string _script;
        private string _trigger;
        private string _message;

        private OverworldUIElement _uiElement;
        private bool _addedUIElement = false;

        private Collider _collider;

        public ScriptTriggerEntityComponent(EntityComponentDataCreationStruct parameters)
            : base(parameters)
        {
            _script = GetDataOrDefault("Script", "");
            _trigger = GetDataOrDefault("Trigger", "Interaction");
            _message = GetDataOrDefault("Message", "Interact");

            _collider = Collider.CreateBoundingBox(Parent.Scale, null);
            _uiElement = new OverworldUIElement();
            _uiElement.Draw += DrawUI;
            _uiElement.Update += UpdateUI;

            _buttonChart = new Pie2D(Game.GraphicsDevice, 24, 0f, 20, Vector2.Zero, false);
            _buttonChart.PrimaryColor = Color.Gray;
            _buttonChart.SecondaryColor = Color.Black;
            _buttonChart.ChartType = PieChartType.RadialFill;
        }

        public override void Update(float elapsedTime)
        {
            var screen = Game.ScreenManager.CurrentScreen;
            if (screen is OverworldScreen)
            {
                var overworldScreen = (OverworldScreen)screen;
                var player = overworldScreen.ActiveWorld.Player;
                var camera = player.Camera;

                _collider.SetPosition(Parent.Position);
                var collisionResult =  _collider.CheckCollision(player.Collider);
                if (collisionResult.Collides)
                {
                    if (!_addedUIElement)
                    {
                        overworldScreen.AddUIElement(_uiElement);
                        _addedUIElement = true;
                    }
                    _uiElement.IsActive = true;

                }
                else
                {
                    _uiElement.IsActive = false;
                }
            }
        }

        private float _buttonPressed = 0f;
        private Pie2D _buttonChart = null;

        private void DrawUI(OverworldUIElement element)
        {
            var overworldScreen = element.Screen;
            var player = overworldScreen.ActiveWorld.Player;
            var camera = player.Camera;

            var position = camera.Viewport.Project(Parent.Position + new Vector3(0, 0.5f, 0), camera.ProjectionMatrix, camera.ViewMatrix, Matrix.Identity);
            var position2D = new Vector2(position.X, position.Y);

            var font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.LargeUIRegular);
            Game.SpriteBatch.DrawString(font, _message, position2D + new Vector2(-(font.MeasureString(_message).X / 2f), 0), Color.White);

            Game.ShapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24, (int)position2D.Y + 32, 48, 48), new Color(0, 0, 0, 180));

            _buttonChart.Position = position2D + new Vector2(-24, 32);
            _buttonChart.DrawBatched(Game.SpriteBatch);

            Game.ShapeRenderer.DrawShape(new Ellipse((int)position2D.X - 24 + 8, (int)position2D.Y + 32 + 8, 32, 32), new Color(64, 200, 64));
            Game.SpriteBatch.Draw(Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.GamePadButtons.Button_A), new Rectangle((int)position2D.X - 24 + 8, (int)position2D.Y + 32 + 8, 32, 32), Color.White);
        }

        private void UpdateUI(OverworldUIElement element)
        {
            if (Game.InputSystem.GamePad.IsButtonDown(Buttons.A))
            {
                _buttonPressed += 0.035f;
                if (_buttonPressed >= 1f)
                {
                    _buttonPressed = 1f;
                    _message = "It worked!";
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
    }
}

