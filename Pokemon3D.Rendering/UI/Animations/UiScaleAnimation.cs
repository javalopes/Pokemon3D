using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiScaleAnimation : UiAnimation
    {
        private readonly Vector2 _startScale;
        private readonly Vector2 _targetScale;

        public UiScaleAnimation(float durationSeconds, Vector2 startScale, Vector2 targetScale) : base(durationSeconds)
        {
            _startScale = startScale;
            _targetScale = targetScale;
        }

        protected override void OnUpdate()
        {
            Owner.Scale = Vector2.Lerp(_startScale, _targetScale, Delta);
        }
    }
}
