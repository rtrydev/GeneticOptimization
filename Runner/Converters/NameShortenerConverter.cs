using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Runner.Converters;

public class NameShortenerConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var input = (string[]) value;
        var result = input.Select(x => x.Split(".").Last());
        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (string[]) value;
    }
}