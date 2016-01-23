using Microsoft.Xna.Framework;

namespace Pokemon3D.UI.Framework
{
    class ColorTransition
    {
        private Color _color;
        private float _speedValue;

        public Color Color
        {
            get { return _color; }
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
            if (_color.R != TargetColor.R)
                _color.R = (byte)MathHelper.Clamp(MathHelper.SmoothStep(TargetColor.R, _color.R, _speedValue), 0, 255);
            if (_color.G != TargetColor.G)
                _color.G = (byte)MathHelper.Clamp(MathHelper.SmoothStep(TargetColor.G, _color.G, _speedValue), 0, 255);
            if (_color.B != TargetColor.B)
                _color.B = (byte)MathHelper.Clamp(MathHelper.SmoothStep(TargetColor.B, _color.B, _speedValue), 0, 255);
        }
    }
}
