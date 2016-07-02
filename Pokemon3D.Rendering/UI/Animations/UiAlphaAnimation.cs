using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiAlphaAnimation : UiAnimation
    {
        public float StartAlpha { get; set; }
        public float EndAlpha { get; set; }

        public UiAlphaAnimation(float durationSeconds, float startAlpha, float endAlpha) : base(durationSeconds)
        {
            StartAlpha = startAlpha;
            EndAlpha = endAlpha;
        }

        protected override void OnUpdate()
        {
            Owner.Alpha = MathHelper.Lerp(StartAlpha, EndAlpha, Delta);
        }
    }
}