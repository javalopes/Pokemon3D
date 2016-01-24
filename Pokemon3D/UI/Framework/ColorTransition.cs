using static System.Math;
using static Microsoft.Xna.Framework.MathHelper;
using Microsoft.Xna.Framework;

namespace Pokemon3D.UI.Framework
{
    /// <summary>
    /// Performs a color transition from a color to a target color at a set speed.
    /// </summary>
    class ColorTransition
    {
        private Color _color;
        private float _speedValue;

        private float _r, _g, _b, _a;
        
        /// <summary>
        /// The current color.
        /// </summary>
        public Color Color
        {
            get { return new Color((byte)Clamp(_r, 0, 255),
                (byte)Clamp(_g, 0, 255),
                (byte)Clamp(_b, 0, 255),
                (byte)Clamp(_a, 0, 255)); }
        }

        public Color TargetColor { get; set; }

        public ColorTransition(Color startColor, float speedValue)
        {
            _color = startColor;
            TargetColor = startColor;
            _speedValue = speedValue;
        }

        public void Update()
        {
            if (_r != TargetColor.R)
            {
                _r = SmoothStep(TargetColor.R, _r, _speedValue);
                if (Abs((float)TargetColor.R - _r) < 10f)
                    _r = TargetColor.R;
            }
            if (_g != TargetColor.G)
            {
                _g = SmoothStep(TargetColor.G, _g, _speedValue);
                if (Abs((float)TargetColor.G - _g) < 10f)
                    _g = TargetColor.G;
            }
            if (_b != TargetColor.B)
            {
                _b = SmoothStep(TargetColor.B, _b, _speedValue);
                if (Abs((float)TargetColor.B - _b) < 10f)
                    _b = TargetColor.B;
            }
            if (_a != TargetColor.A)
            {
                _a = SmoothStep(TargetColor.A, _a, _speedValue);
                if (Abs((float)TargetColor.A - _a) < 10f)
                    _a = TargetColor.A;
            }
        }

        public bool Finished
        {
            get
            {
                return Color == TargetColor;
            }
        }
    }
}
