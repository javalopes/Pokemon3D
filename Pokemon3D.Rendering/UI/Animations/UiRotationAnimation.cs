using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokemon3D.Rendering.UI.Animations
{
    public class UiRotationAnimation : UiAnimation
    {
        private readonly float _startRotation;
        private readonly float _endRotation;

        public UiRotationAnimation(float durationSeconds, float startRotation, float endRotation) : base(durationSeconds)
        {
            _startRotation = startRotation;
            _endRotation = endRotation;
        }

        protected override void OnUpdate()
        {
            Owner.Rotation = MathHelper.Lerp(_startRotation, _endRotation, Delta);
        }
    }
}
