using System;

namespace Pokemon3D.Rendering.UI
{
    public abstract class UiAnimation : Common.Animations.Animation
    {
        public virtual UiElement Owner { get; set; }

        protected UiAnimation(float durationSeconds) : base(durationSeconds, false)
        {
        }

        protected override void OnUpdate()
        {
            var delta = Math.Min(ElapsedSeconds/DurationSeconds, 1.0f);
            OnUpdateDelta(delta);
        }

        public abstract void OnUpdateDelta(float delta);
    }
}