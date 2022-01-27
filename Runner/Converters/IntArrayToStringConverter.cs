using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Runner.Converters;

public class IntArrayToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var array = (int[]) value;
        var result = "";
        for (int i = 0; i < array.Length - 1; i++)
        {
            result += $"{array[i]}, ";
        }

        result += array[^1];
        return result;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}