using System.Collections.Generic;

namespace Pokemon3D.Rendering.UI
{
    public class UiMultiAnimation : UiAnimation
    {
        private readonly List<UiAnimation> _animations;

        public UiMultiAnimation(float durationSeconds, IEnumerable<UiAnimation> animations) : base(durationSeconds)
        {
            _animations = new List<UiAnimation>(animations);
        }

        public override void OnUpdateDelta(float delta)
        {
            _animations.ForEach(a => a.OnUpdateDelta(delta));
        }
    }
}