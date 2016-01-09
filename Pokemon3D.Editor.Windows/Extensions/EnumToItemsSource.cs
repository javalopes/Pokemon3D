using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Pokemon3D.Editor.Windows.Extensions
{
    class EnumTypeToValuesConverter : IValueConverter
    {
        public static EnumTypeToValuesConverter Instance = new EnumTypeToValuesConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type) return Enum.GetValues((Type) value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
