using System.Globalization;

namespace FinanControl.App.Converters
{
    public class SaldoToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal saldo)
            {
                if (saldo > 0)
                    return Color.FromArgb("#4CAF50"); // Verde
                else if (saldo < 0)
                    return Color.FromArgb("#F44336"); // Vermelho
                else
                    return Color.FromArgb("#FFC107"); // Amarelo
            }
            return Color.FromArgb("#9E9E9E");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
