using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiColorAnimation : UiAnimation
    {
        private readonly Color _startColor;
        private readonly Color _endColor;

        public UiColorAnimation(float durationSeconds, Color startColor, Color endColor) : base(durationSeconds)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        public override void OnUpdateDelta(float delta)
        {
            Owner.Color = Color.Lerp(_startColor, _endColor, delta);
        }
    }
}