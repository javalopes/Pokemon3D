using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Performs a transition between two float values.
    /// </summary>
    class OffsetTransition
    {
        /// <summary>
        /// The current value.
        /// </summary>
        public float Offset { get; private set; }

        public float TargetOffset { get; set; }

        public float Speed { get; set; }
        
        public OffsetTransition(float offset, float speedValue)
        {
            Offset = offset;
            TargetOffset = offset;
            Speed = speedValue;
        }

        public void Update()
        {
            Offset = MathHelper.SmoothStep(TargetOffset, Offset, Speed);
            if (Math.Abs(Offset - TargetOffset) < 0.1f)
                Offset = TargetOffset;
        }
    }
}
