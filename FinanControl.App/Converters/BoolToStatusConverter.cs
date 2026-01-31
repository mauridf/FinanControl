using System.Globalization;

namespace FinanControl.App.Converters;

public class BoolToStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Efetivada" : "Pendente";
        }
        return "Indefinido";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status == "Efetivada";
        }
        return false;
    }
}