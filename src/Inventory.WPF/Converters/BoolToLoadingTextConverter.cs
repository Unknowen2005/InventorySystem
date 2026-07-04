using System;
using System.Globalization;
using System.Windows.Data;

namespace Inventory.WPF.Converters;

public class BoolToLoadingTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isLoading && isLoading)
            return "Carregando...";

        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}