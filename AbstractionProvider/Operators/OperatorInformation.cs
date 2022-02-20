using System.Reflection;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Operators;
using CodeCompiler;

namespace AbstractionProvider.Operators;

public class OperatorInformation
{
    public string[] Available => GetAvailableOperators();
    public OperatorTypes OperatorType { get; set; }
    public string OperatorName { get; set; }

    public OperatorInformation(OperatorTypes type, string name)
    {
        OperatorName = name;
        OperatorType = type;
    }

    public string[] GetAvailableOperators()
    {
        var selections = ClassProvider.GetAllClassNames(typeof(Selection));
        var crossovers = ClassProvider.GetAllClassNames(typeof(Crossover));
        var eliminations = ClassProvider.GetAllClassNames(typeof(Elimination));
        var mutations = ClassProvider.GetAllClassNames(typeof(Mutation));

        switch (OperatorType)
        {
            case OperatorTypes.Selection: return selections;
            case OperatorTypes.Crossover: return crossovers;
            case OperatorTypes.Elimination: return eliminations;
            case OperatorTypes.Mutation: return mutations;
            default: return null;
        }
    }
}