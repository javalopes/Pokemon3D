using Microsoft.Xna.Framework;
using System.Globalization;

namespace Pokemon3D.Common.Extensions
{
    public static class ColorExtensions
    {
        public static Color ParseColor(this string value)
        {
            var alpha = value.Substring(0, 2);
            var red = value.Substring(2, 2);
            var green = value.Substring(4, 2);
            var blue = value.Substring(6, 2);

            return new Color(int.Parse(alpha, NumberStyles.HexNumber),
                             int.Parse(red, NumberStyles.HexNumber),
                             int.Parse(green, NumberStyles.HexNumber),
                             int.Parse(blue, NumberStyles.HexNumber));
        }

        public static Color Alpha(this Color color, byte alpha)
        {
            color.A = alpha;
            return color;
        }
        public static Color Alpha(this Color color, int alpha)
        {
            color.A = (byte)alpha;
            return color;
        }
        public static Color Alpha(this Color color, float alpha)
        {
            color.A = (byte)(alpha * 255);
            return color;
        }
    }
}
