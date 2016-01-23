using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    class OffsetTransition
    {
        public float Offset { get; private set; }

        public float TargetOffset { get; set; }

        private float _speedValue;

        public OffsetTransition(float offset, float speedValue)
        {
            Offset = offset;
            TargetOffset = offset;
            _speedValue = speedValue;
        }

        public void Update()
        {
            Offset = MathHelper.SmoothStep(TargetOffset, Offset, _speedValue);
            if (Math.Abs(Offset - TargetOffset) < 0.1f)
                Offset = TargetOffset;
        }
    }
}
