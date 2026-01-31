using FinanControl.Core.Entities;
using System.Globalization;

namespace FinanControl.App.Converters;

public class CategoriaToIdConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int id && id > 0)
        {
            // Retornar a categoria pelo ID (isso será tratado no ViewModel)
            return null;
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Categoria categoria)
        {
            return categoria.Id;
        }
        return 0;
    }
}