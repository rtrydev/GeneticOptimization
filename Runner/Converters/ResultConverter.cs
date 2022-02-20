using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Runner.Converters;

public class ResultConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var val = (string) value;
        var place = -1;

        for (int i = 0; i < 2; i++)
        {
            place = val.LastIndexOf("_");
            val = val.Remove(place, 1).Insert(place, ":");
            place = val.LastIndexOf("-");
            val = val.Remove(place, 1).Insert(place, " ");
        }

        place = val.LastIndexOf("_");
        val = val.Remove(place, 1).Insert(place, " ");

        var split = val.Split(" ");
        split[^2] = DateConverter.GetMonthString(split[^2]);

        var result = string.Join(" ", split);

        result = result.Replace(".json", "");
        return result;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}