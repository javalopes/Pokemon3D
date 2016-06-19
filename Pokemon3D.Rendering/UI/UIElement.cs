using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElement
    {
        private readonly Animator _animator;
        private UiAnimation _enterAnimation;
        private UiAnimation _leaveAnimation;
        private UiAnimation _hoverAnimation;
        private readonly Texture2D _texture;

        public Rectangle Bounds { get; protected set; }
        public int TabIndex { get; set; }
        public Color Color { get; set; }
        public Vector2 Offset { get; set; }
        public float Alpha { get; set; }
        public UiState State { get; private set; }
        public Rectangle SourceRectangle { get; set; }

        private void UpdateAnimation(string name, ref UiAnimation backingField, UiAnimation newValue)
        {
            if (backingField == newValue) return;
            _animator.RemoveAnimation(name);
            _enterAnimation = newValue;
            _animator.AddAnimation(name, _enterAnimation);
        }

        protected UiElement(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            _texture = texture;
            SourceRectangle = sourceRectangle.GetValueOrDefault(_texture.Bounds);
            Alpha = 1.0f;
            Color = Color.White;
            TabIndex = 0;
            Offset = Vector2.Zero;

            _animator = new Animator();
            _animator.AnimationFinished += OnAnimationFinished;
            State = UiState.Inactive;
        }

        public void ScaleUniformToHeight(int height)
        {
            var percentageHeight = height/(float) SourceRectangle.Height;
            var bounds = Bounds;
            bounds.Height = height;
            bounds.Width = (int) Math.Round(SourceRectangle.Width*percentageHeight, MidpointRounding.AwayFromZero);
            Bounds = bounds;
        }

        public UiAnimation EnterAnimation
        {
            get { return _enterAnimation; }
            set { UpdateAnimation("Enter", ref _enterAnimation, value); }
        }

        public UiAnimation LeaveAnimation
        {
            get { return _leaveAnimation; }
            set { UpdateAnimation("Leave", ref _leaveAnimation, value); }
        }

        public UiAnimation HoverAnimation
        {
            get { return _hoverAnimation; }
            set { UpdateAnimation("Hover", ref _hoverAnimation, value); }
        }

        private void OnAnimationFinished(string animationName)
        {
            switch (animationName)
            {
                case "Enter":
                    State = UiState.Active;
                    break;
                case "Leave":
                    State = UiState.Inactive;
                    break;
            }
        }

        public virtual void Show()
        {
            if (EnterAnimation != null)
            {
                State = UiState.Entering;
                _animator.SetAnimation("Enter");
            }
            else
            {
                State = UiState.Active;
            }
        }

        public virtual void Hide()
        {
            if (LeaveAnimation != null)
            {
                State = UiState.Leaving;
                _animator.SetAnimation("Leave");
            }
            else
            {
                State = UiState.Inactive;
            }
        }

        public virtual void Update(GameTime time)
        {
            _animator.Update(time);
        }

        protected void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, SourceRectangle, Color * Alpha, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
