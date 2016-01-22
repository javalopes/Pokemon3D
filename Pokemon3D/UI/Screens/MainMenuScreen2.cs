using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Input;
using Pokemon3D.Common.Input;
using Pokemon3D.UI.Transitions;

namespace Pokemon3D.UI.Screens
{
    class MainMenuScreen2 : GameObject, Screen
    {
        private ControlGroup _buttons = new ControlGroup();

        public void OnOpening(object enterInformation)
        {
            _buttons.Add(new LeftSideButton(_buttons, "Start new game", new Vector2(50, 50), b =>
            {
                Game.ScreenManager.SetScreen(typeof(GameModeLoadingScreen), typeof(BlendTransition));
            }));
            _buttons.Add(new LeftSideButton(_buttons, "Load game", new Vector2(50, 110), null));
            _buttons.Add(new LeftSideButton(_buttons, "GameJolt", new Vector2(50, 170), null));
            _buttons.Add(new LeftSideButton(_buttons, "Options", new Vector2(50, 230), null));
            _buttons.Add(new LeftSideButton(_buttons, "Exit game", new Vector2(50, 290), (b) =>
            {
                Game.ScreenManager.NotifyQuitGame();
            }));
        }

        public void OnClosing()
        {

        }

        public void OnDraw(GameTime gameTime)
        {
            GameController.Instance.GraphicsDevice.Clear(Color.LightGray);
            GameController.Instance.SpriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            _buttons.Draw();

            Game.ShapeRenderer.DrawFilledRectangle(0, Game.ScreenBounds.Height - 50, Game.ScreenBounds.Width, 50, new Color(255, 255, 255, 100));
            GameController.Instance.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {
            _buttons.Update();
        }

        private class ControlGroup : List<Control>
        {
            public void FocusChange(int change)
            {
                if (!this.Any(x => x.Selected))
                {
                    this[0].Select();
                }
                else
                {
                    int currentIndex = IndexOf(this.Single(x => x.Selected));
                    currentIndex += change;
                    while (currentIndex < 0)
                        currentIndex += Count;
                    while (currentIndex >= Count)
                        currentIndex -= Count;

                    this[currentIndex].Select();
                }
            }

            public void Update()
            {
                if (GameController.Instance.InputSystem.Up(true, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.WASD | DirectionalInputTypes.ScrollWheel))
                {
                    FocusChange(-1);
                }
                if (GameController.Instance.InputSystem.Down(true, DirectionalInputTypes.ArrowKeys | DirectionalInputTypes.WASD | DirectionalInputTypes.ScrollWheel))
                {
                    FocusChange(1);
                }

                ForEach(b => b.Update());
            }

            public void Draw()
            {
                ForEach(b => b.Draw());
            }
        }

        abstract private class Control : GameObject
        {
            protected ControlGroup Group { get; private set; }
            public bool Selected { get; protected set; }

            public Control(ControlGroup group)
            {
                Group = group;
                Selected = false;
            }

            public abstract void Update();

            public abstract void Draw();

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
            }
        }

        private class LeftSideButton : Control
        {
            private Texture2D _texture;
            private SpriteFont _font;

            private Vector2 _initialPosition;
            private Action<LeftSideButton> _onClick;

            private float _offset = 0;
            private float _targetOffset = 0;

            private Color _color = new Color(255, 255, 255);
            private Color _targetColor = new Color(255, 255, 255);

            public string Text { get; set; }

            public LeftSideButton(ControlGroup group, string text, Vector2 position, Action<LeftSideButton> onClick) : base(group)
            {
                _texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);
                _font = Game.Content.Load<SpriteFont>(ResourceNames.Fonts.NormalFont);

                Text = text;
                _initialPosition = position;
                _onClick = onClick;

            }

            private Rectangle GetRectangle()
            {
                return new Rectangle((int)(_initialPosition.X + _offset), (int)_initialPosition.Y, 200, 36);
            }

            public override void Update()
            {
                if (Game.InputSystem.Mouse.HasMoved)
                    if (GetRectangle().Contains(Game.InputSystem.Mouse.Position))
                        Select();

                if (_onClick != null && Selected)
                {
                    if (Game.InputSystem.Accept(AcceptInputTypes.Buttons) ||
                        GetRectangle().Contains(Game.InputSystem.Mouse.Position) && Game.InputSystem.Accept(AcceptInputTypes.LeftClick))
                    {
                        _onClick(this);
                    }
                }

                UpdateOffset();
                UpdateColor();
            }

            private void UpdateOffset()
            {
                _offset = MathHelper.SmoothStep(_targetOffset, _offset, 0.5f);
                if (Math.Abs(_offset - _targetOffset) < 0.1f)
                {
                    _offset = _targetOffset;
                }
            }

            private void UpdateColor()
            {
                if (_color.R != _targetColor.R)
                    _color.R = (byte)MathHelper.Clamp(MathHelper.SmoothStep(_targetColor.R, _color.R, 0.5f), 0, 255);
                if (_color.G != _targetColor.G)
                    _color.G = (byte)MathHelper.Clamp(MathHelper.SmoothStep(_targetColor.G, _color.G, 0.5f), 0, 255);
                if (_color.B != _targetColor.B)
                    _color.B = (byte)MathHelper.Clamp(MathHelper.SmoothStep(_targetColor.B, _color.B, 0.5f), 0, 255);
            }

            public override void Select()
            {
                base.Select();

                _targetOffset = 26;
                _targetColor = new Color(255, 217, 102);
            }

            public override void Deselect()
            {
                base.Deselect();

                _targetOffset = 0;
                _targetColor = new Color(255, 255, 255);
            }

            public override void Draw()
            {
                Game.SpriteBatch.Draw(_texture, GetRectangle(), _color);
                Game.SpriteBatch.DrawString(_font, Text, new Vector2(_initialPosition.X + _offset + 32, _initialPosition.Y + 9), Color.Black);

                Game.ShapeRenderer.DrawCircle((int)(_initialPosition.X + _offset + 18), (int)_initialPosition.Y + 16, 10, 6, Color.Black);
                if (Selected)
                    Game.ShapeRenderer.DrawCircle((int)(_initialPosition.X + _offset + 18), (int)_initialPosition.Y + 16, 3, 3, Color.Black);
            }
        }
    }
}
