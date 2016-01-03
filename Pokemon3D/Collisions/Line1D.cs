using System;

namespace Pokemon3D.Collisions
{
    struct Line1D
    {
        public float Min;
        public float Max;

        public Line1D(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float OverlapSize(Line1D other)
        {
            return Math.Max(0, Math.Min(Max, other.Max) - Math.Max(Min, other.Max));
        }
    }
}