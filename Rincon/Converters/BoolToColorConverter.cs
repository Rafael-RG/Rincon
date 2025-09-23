using System;
using System.Globalization;

namespace Rincon.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public static readonly BoolToColorConverter Instance = new BoolToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue && parameter is string param)
            {
                var colors = param.Split('|'); // Separar los colores por "|"
                return booleanValue ? colors[0] : colors[1];
            }
            return "#808080"; // Color gris por defecto
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}