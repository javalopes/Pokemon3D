using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.Linq;

namespace Pokemon3D.Entities.System
{
    /// <summary>
    /// Contains static methods to convert the string data of entity components to other data types.
    /// </summary>
    internal static class TypeConverter
    {

        public static T Convert<T>(string data)
        {
            switch (typeof(T).Name)
            {
                case "Int32":
                    return (T)ToInt32(data);
                case "Vector3":
                    return (T)ToVector3(data);
                case "Vector2":
                    return (T)ToVector2(data);
                case "Single":
                    return (T)ToSingle(data);
                case "Rectangle":
                    return (T)ToRectangle(data);
                case "Int32[]":
                    return (T)ToArray<int>(data);
                case "String":
                    return (T)ToString(data);
                case "Boolean":
                    return (T)ToBoolean(data);
                default:
                    return default(T);
            }
        }

        private static object ToBoolean(string data)
        {
            return bool.Parse(data);
        }

        private static object ToRectangle(string data)
        {
            var token = data.Split(',');
            return new Rectangle(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));
        }

        private static object ToArray<T>(string data)
        {
            return data.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(Convert<T>).ToArray();
        }

        private static object ToVector3(string data)
        {
            string[] dataParts = data.Split(',');

            if (dataParts.Length == 3)
            {
                float xResult, yResult, zResult;

                if (float.TryParse(dataParts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out xResult))
                    if (float.TryParse(dataParts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out yResult))
                        if (float.TryParse(dataParts[2], NumberStyles.Number, CultureInfo.InvariantCulture, out zResult))
                            return new Vector3(xResult, yResult, zResult);

            }

            return Vector3.Zero;
        }

        private static object ToString(string data)
        {
            return data;
        }

        private static object ToVector2(string data)
        {
            string[] dataParts = data.Split(',');

            if (dataParts.Length == 2)
            {
                float xResult, yResult;

                if (float.TryParse(dataParts[0], NumberStyles.Number, CultureInfo.InvariantCulture, out xResult))
                    if (float.TryParse(dataParts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out yResult))
                        return new Vector2(xResult, yResult);

            }

            return Vector2.Zero;
        }

        private static object ToSingle(string data)
        {
            float result;
            float.TryParse(data, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
            return result;
        }

        private static object ToInt32(string data)
        {
            int result;
            int.TryParse(data, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
            return result;
        }

    }
}
