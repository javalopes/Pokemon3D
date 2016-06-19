using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElement : UiBaseElement
    {
        private readonly Animator _animator;
        private UiAnimation _enterAnimation;
        private UiAnimation _leaveAnimation;
        private UiAnimation _hoverAnimation;
        private readonly Texture2D _texture;

        public Rectangle Bounds { get; protected set; }
        public int TabIndex { get; set; }
        public Rectangle SourceRectangle { get; set; }

        private void UpdateAnimation(string name, ref UiAnimation backingField, UiAnimation newValue)
        {
            if (backingField == newValue) return;
            _animator.RemoveAnimation(name);
            backingField = newValue;
            backingField.Owner = this;
            _animator.AddAnimation(name, backingField);
        }

        protected UiElement(Texture2D texture, Rectangle? sourceRectangle = null)
        {
            _texture = texture;
            SourceRectangle = sourceRectangle.GetValueOrDefault(_texture.Bounds);
            Bounds = SourceRectangle;
            Alpha = 1.0f;
            Color = Color.White;
            TabIndex = 0;
            Offset = Vector2.Zero;

            _animator = new Animator();
            _animator.AnimationFinished += OnAnimationFinished;
            State = UiState.Inactive;
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

        public override void Show()
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

        public override void Hide()
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

        public override void Hover()
        {
            if (State != UiState.Active) return;
            if (HoverAnimation != null)
            {
                _animator.SetAnimation("Hover");
            }
            else
            {
                State = UiState.Hover;
            }
        }

        public override void Unhover()
        {
            if (State != UiState.Hover) return;
            if (HoverAnimation != null)
            {
                _animator.SetAnimation("Hover");
            }
            else
            {
                State = UiState.Active;
            }
        }

        public override void Update(GameTime time)
        {
            _animator.Update(time);
        }

        protected void DrawTexture(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, SourceRectangle, Color * Alpha, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
