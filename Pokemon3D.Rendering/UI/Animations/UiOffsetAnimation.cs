using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiOffsetAnimation : UiAnimation
    {
        private readonly Vector2 _offsetStart;
        private readonly Vector2 _offsetEnd;

        public UiOffsetAnimation(float durationSeconds, Vector2 offsetStart, Vector2 offsetEnd) : base(durationSeconds)
        {
            _offsetStart = offsetStart;
            _offsetEnd = offsetEnd;
        }

        public override void OnUpdateDelta(float delta)
        {
            Owner.Offset = Vector2.Lerp(_offsetStart, _offsetEnd, delta);
        }
    }
}