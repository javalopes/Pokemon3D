using System.Collections.Generic;
using System.Linq;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiMultiAnimation : UiAnimation
    {
        private readonly List<UiAnimation> _animations;
        private UiElement _owner;

        public UiMultiAnimation(UiAnimation[] animations) : base(animations.Max(a => a.DurationSeconds + a.Delay))
        {
            _animations = new List<UiAnimation>(animations);
        }

        public override void Start(bool playReversed = false)
        {
            base.Start(playReversed);
            _animations.ForEach(a => a.Start(playReversed));
        }

        public override void Update(float elapsedSeconds)
        {
            if (IsFinished) return;
            foreach (var uiAnimation in _animations) uiAnimation.Update(elapsedSeconds);
            IsFinished = _animations.All(a => a.IsFinished);
        }

        protected override void OnUpdate()
        {
        }

        public override UiElement Owner
        {
            get { return _owner; }
            set
            {
                _owner = value;
                _animations.ForEach(a => a.Owner = value);
            }
        }
    }
}