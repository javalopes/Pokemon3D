using System.Collections.Generic;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiMultiAnimation : UiAnimation
    {
        private readonly List<UiAnimation> _animations;
        private UiElement _owner;

        public UiMultiAnimation(float durationSeconds, IEnumerable<UiAnimation> animations) : base(durationSeconds)
        {
            _animations = new List<UiAnimation>(animations);
        }

        public override void OnUpdateDelta(float delta)
        {
            _animations.ForEach(a => a.OnUpdateDelta(delta));
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