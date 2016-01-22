using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.GameCore;
using Microsoft.Xna.Framework.Input;

namespace Pokemon3D.UI.Screens
{
    class MainMenuScreen2 : GameObject, Screen
    {
        private List<Control> _buttons = new List<Control>();

        public void OnOpening(object enterInformation)
        {
            _buttons.Add(new LeftSideButton(_buttons, "Start new game", new Vector2(50, 50), null));
            _buttons.Add(new LeftSideButton(_buttons, "Continue", new Vector2(50, 110), null));
            _buttons.Add(new LeftSideButton(_buttons, "Continue", new Vector2(50, 170), null));
            _buttons.Add(new LeftSideButton(_buttons, "Continue", new Vector2(50, 230), null));
        }

        public void OnClosing()
        {

        }

        public void OnDraw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);
            Game.SpriteBatch.Begin();
            _buttons.ForEach(b => b.Draw());
            Game.SpriteBatch.End();
        }

        public void OnUpdate(float elapsedTime)
        {
            _buttons.ForEach(b => b.Update());
        }

        abstract private class Control : GameObject
        {
            protected List<Control> Group { get; private set; }
            public bool Selected { get; protected set; }

            public Control(List<Control> group)
            {
                Group = group;
                Selected = false;
            }

            public abstract void Update();

            public abstract void Draw();

            public abstract void Deselect();

            public virtual void Select()
            {
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
            private string _text;
            private Vector2 _initialPosition;
            private Action _onClick;

            private float _offset = 0;
            private float _targetOffset = 0;

            private Color _color = new Color(255, 255, 255);
            private Color _targetColor = new Color(255, 255, 255);

            public LeftSideButton(List<Control> group, string text, Vector2 position, Action onClick) : base(group)
            {
                _texture = Game.Content.Load<Texture2D>(ResourceNames.Textures.UI.Common.Button_Blank);

                _text = text;
                _initialPosition = position;
                _onClick = onClick;

            }

            private Rectangle GetRectangle()
            {
                return new Rectangle((int)(_initialPosition.X + _offset), (int)_initialPosition.Y, 200, 36);
            }

            public override void Update()
            {
                MouseState mState = Mouse.GetState();

                if (GetRectangle().Contains(mState.Position))
                    Select();

                UpdateOffset();
                UpdateColor();
            }

            private void UpdateOffset()
            {
                _offset = MathHelper.SmoothStep(_targetOffset, _offset, 0.5f);
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

                _targetOffset = 50;
                _targetColor = new Color(255, 217, 102);
            }

            public override void Deselect()
            {
                _targetOffset = 0;
                _targetColor = new Color(255, 255, 255);
            }

            public override void Draw()
            {
                Game.SpriteBatch.Draw(_texture, GetRectangle(), _color);
            }
        }
    }
}
