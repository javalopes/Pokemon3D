using System;

namespace Pokemon3D.Rendering.UI.Animations
{
    public abstract class UiAnimation : Common.Animations.Animation
    {
        public virtual UiElement Owner { get; set; }

        protected UiAnimation(float durationSeconds) : base(durationSeconds, false)
        {
        }
    }
}