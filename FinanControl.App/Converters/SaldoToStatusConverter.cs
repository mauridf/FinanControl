using System.Globalization;

namespace FinanControl.App.Converters
{
    public class SaldoToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal saldo)
            {
                if (saldo > 0)
                    return "Positivo";
                else if (saldo < 0)
                    return "Negativo";
                else
                    return "Neutro";
            }
            return "Indefinido";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "Positivo" => 100m,    // Valor exemplo
                    "Negativo" => -100m,   // Valor exemplo
                    "Neutro" => 0m,
                    _ => 0m
                };
            }
            return 0m;
        }
    }
}
