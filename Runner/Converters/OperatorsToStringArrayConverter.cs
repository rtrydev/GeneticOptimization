using System;
using System.Globalization;
using Avalonia.Data.Converters;
using GeneticOptimization.Operators;

namespace Runner.Converters;

public class OperatorsToStringArrayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not OperatorInformation[]) return null;
        var operators = (OperatorInformation[]) value;
        var result = new string[operators.Length];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = $"{operators[i].OperatorName}";
        }

        return result;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}