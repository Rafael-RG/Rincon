using System;
using System.Globalization;


namespace Rincon.Converters
{
     public class BoolToTextConverter : IValueConverter
    {     public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool booleanValue && parameter is string param)
                {
                    var texts = param.Split('|'); // Separar los textos por "|"
                    return booleanValue ? texts[0] : texts[1];
                }
                return "Valor inválido";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value is string text && parameter is string param && text == param.Split('|')[0];
            }
    }
}