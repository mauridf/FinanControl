using System.Globalization;

namespace FinanControl.App.Converters;

public class BoolToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Color.FromArgb("#4CAF50") : Color.FromArgb("#FF9800");
        }
        return Color.FromArgb("#9E9E9E");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}