using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Rendering.UI.Animations;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiElement
    {
        private readonly Animator _animator;
        private UiAnimation _enterAnimation;
        private UiAnimation _leaveAnimation;
        private UiAnimation _focusAnimation;

        public Color Color { get; set; }
        public Vector2 Offset { get; set; }
        public float Alpha { get; set; }
        public UiState State { get; protected set; }
        public Rectangle Bounds { get; protected set; }
        public int TabIndex { get; set; }
        public Rectangle? SourceRectangle { get; set; }

        private void UpdateAnimation(string name, ref UiAnimation backingField, UiAnimation newValue)
        {
            if (backingField == newValue) return;
            _animator.RemoveAnimation(name);
            backingField = newValue;
            backingField.Owner = this;
            _animator.AddAnimation(name, backingField);
        }

        protected UiElement()
        {
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

        public UiAnimation FocusedAnimation
        {
            get { return _focusAnimation; }
            set { UpdateAnimation("Focused", ref _focusAnimation, value); }
        }

        private void OnAnimationFinished(string animationName, bool playedReversed)
        {
            switch (animationName)
            {
                case "Enter":
                    State = UiState.Active;
                    break;
                case "Leave":
                    State = UiState.Inactive;
                    break;
                case "Focused":
                    State = playedReversed ? UiState.Active : UiState.Focused;
                    break;
            }
        }

        public void Show()
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

        public void Hide()
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

        public void Focus()
        {
            if (State != UiState.Active) return;
            if (FocusedAnimation != null)
            {
                _animator.SetAnimation("Focused");
            }
            else
            {
                State = UiState.Focused;
            }
        }

        public void Unfocus()
        {
            if (FocusedAnimation != null)
            {
                _animator.SetAnimation("Focused", true);
            }
            else
            {
                State = UiState.Active;
            }
        }

        public void Update(GameTime time)
        {
            _animator.Update(time);
        }

        public bool IsAnimating => _animator.CurrentAnimation != null;

        protected void DrawTexture(SpriteBatch spriteBatch, Texture2D texture)
        {
            var bounds = Bounds;
            bounds.X += (int)Offset.X;
            bounds.Y += (int)Offset.Y;
            spriteBatch.Draw(texture, bounds, SourceRectangle, Color * Alpha, 0.0f, Vector2.Zero, SpriteEffects.None, 0);
        }

        public abstract void OnAction();

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
