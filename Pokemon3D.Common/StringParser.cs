using Microsoft.Xna.Framework;
using System;

namespace Pokemon3D.Common
{
    public static class StringParser
    {
        public static Rectangle ParseRectangle(string value)
        {
            var token = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (token.Length < 4) return new Rectangle();

            return new Rectangle(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));
        }
    }
}
