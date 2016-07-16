using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pokemon3D.Common.Animations;
using Pokemon3D.Rendering.UI.Animations;

namespace Pokemon3D.Rendering.UI
{
    /// <summary>
    /// Represents a single drawable UI Element.
    /// </summary>
    public abstract class UiElement
    {
        private readonly Animator _animator;
        private UiAnimation _enterAnimation;
        private UiAnimation _leaveAnimation;
        private UiAnimation _focusAnimation;

        /// <summary>
        /// Modulative Color of Element. Can be used for animation.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Offset to their position. For animation purposes.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Origin from where the position of the bounds defines.
        /// </summary>
        protected Vector2 Origin { get; private set; }

        /// <summary>
        /// Alpha-Value of element.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Current State. Mostly for internal usage.
        /// </summary>
        public UiState State { get; protected set; }

        /// <summary>
        /// Bounds of Element takes space.
        /// </summary>
        public Rectangle Bounds { get; protected set; }

        /// <summary>
        /// Tab index. Defines order for focusing.
        /// </summary>
        public int TabIndex { get; set; }

        private void UpdateAnimation(string name, ref UiAnimation backingField, UiAnimation newValue)
        {
            if (backingField == newValue) return;
            _animator.RemoveAnimation(name);
            backingField = newValue;
            AddAnimation(name, backingField);
        }

        protected TAnimation AddAnimation<TAnimation>(string name, TAnimation animation) where TAnimation : UiAnimation
        {
            animation.Owner = this;
            _animator.AddAnimation(name, animation);
            return animation;
        }

        protected void PlayAnimation(string name)
        {
            _animator.SetAnimation(name);
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
            Scale = Vector2.One;
        }

        /// <summary>
        /// Animation for entering when called Show().
        /// </summary>
        public UiAnimation EnterAnimation
        {
            get { return _enterAnimation; }
            set { UpdateAnimation("Enter", ref _enterAnimation, value); }
        }

        /// <summary>
        /// Animation for leaving when called Hide().
        /// </summary>
        public UiAnimation LeaveAnimation
        {
            get { return _leaveAnimation; }
            set { UpdateAnimation("Leave", ref _leaveAnimation, value); }
        }

        /// <summary>
        /// Animation when element gots the focus.
        /// </summary>
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
                default:
                    CustomAnimationFinshed?.Invoke(animationName);
                    break;
            }
        }

        /// <summary>
        /// Shows the active element.
        /// </summary>
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

        /// <summary>
        /// Hides the active element.
        /// </summary>
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

        /// <summary>
        /// Focuses the active element.
        /// </summary>
        public virtual void Focus()
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

        /// <summary>
        /// Unfocus the active element.
        /// </summary>
        public virtual void Unfocus()
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

        /// <summary>
        /// Updates animation.
        /// </summary>
        /// <param name="time">Game time</param>
        public virtual void Update(GameTime time)
        {
            _animator.Update(time);
        }

        /// <summary>
        /// if anything is in animation. This can be used to wait for animation finished.
        /// </summary>
        public virtual bool IsAnimating => _animator.CurrentAnimation != null;

        /// <summary>
        /// Calculates the bounds with offset and origin.
        /// </summary>
        /// <returns>Bounds.</returns>
        public Rectangle GetBounds()
        {
            var bounds = Bounds;
            bounds.X += (int)(Offset.X);
            bounds.Y += (int)(Offset.Y);
            return bounds;
        }

        public void SetPosition(Vector2 position)
        {
            var bounds = Bounds;
            bounds.X = (int)(position.X - Origin.X);
            bounds.Y = (int)(position.Y - Origin.Y);
            Bounds = bounds;
        }

        /// <summary>
        /// percentage offset of where to position the uielement.
        /// This  takes also rotation and scaling center into consideration.
        /// </summary>
        /// <param name="percentage">Percentage from 0 to 1 (clamped)</param>
        public void SetOriginPercentage(Vector2 percentage)
        {
            var originX = Bounds.Width*MathHelper.Clamp(percentage.X, 0, 1);
            var originY = Bounds.Height*MathHelper.Clamp(percentage.Y, 0, 1);
            Origin = new Vector2(originX, originY);
        }

        /// <summary>
        /// Is called when the element is interactable and the action key is pressed.
        /// </summary>
        public virtual void OnAction()
        {
            
        }

        public void AddCustomAnimation(string name, UiAnimation animation)
        {
            animation.Owner = this;
            _animator.AddAnimation(name, animation);
        }

        public void PlayCustomAnimation(string name)
        {
            _animator.SetAnimation(name);
        }

        /// <summary>
        /// If the element can get focus.
        /// </summary>
        public abstract bool IsInteractable { get; }

        /// <summary>
        /// Scale of control.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Called when an animation has finished.
        /// </summary>
        public event Action<string> CustomAnimationFinshed;

        /// <summary>
        /// Rotation of control.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Draws the element
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
