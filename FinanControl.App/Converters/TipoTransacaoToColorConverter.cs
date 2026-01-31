using FinanControl.Core.Enums;
using System.Globalization;

namespace FinanControl.App.Converters;

public class TipoTransacaoToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TipoTransacao tipo)
        {
            return tipo switch
            {
                TipoTransacao.Receita => Color.FromArgb("#4CAF50"), // Verde
                TipoTransacao.Despesa => Color.FromArgb("#F44336"), // Vermelho
                TipoTransacao.Transferencia => Color.FromArgb("#2196F3"), // Azul
                _ => Color.FromArgb("#9E9E9E") // Cinza
            };
        }
        return Color.FromArgb("#9E9E9E");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Color color)
        {
            var hexColor = color.ToHex().ToUpper();

            return hexColor switch
            {
                "#4CAF50" => TipoTransacao.Receita,     // Verde
                "#F44336" => TipoTransacao.Despesa,     // Vermelho  
                "#2196F3" => TipoTransacao.Transferencia, // Azul
                _ => TipoTransacao.Despesa             // Padrão
            };
        }

        return TipoTransacao.Despesa;
    }
}