using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Runner.Converters;

public class DoubleToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var dbl = (double) value;
        return dbl.ToString("0.##");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}