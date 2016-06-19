using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI
{
    public class UiAlphaAnimation : UiAnimation
    {
        private readonly float _startAlpha;
        private readonly float _endAlpha;

        public UiAlphaAnimation(float durationSeconds, float startAlpha, float endAlpha) : base(durationSeconds)
        {
            _startAlpha = startAlpha;
            _endAlpha = endAlpha;
        }

        public override void OnUpdateDelta(float delta)
        {
            Owner.Alpha = MathHelper.Lerp(_startAlpha, _endAlpha, delta);
        }
    }
}