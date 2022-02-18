namespace GeneticOptimization.Operators;

public class OperatorInformation
{
    public string[] Selections { get; } = {"Random", "Roulette", "Elitism"};
    public string[] Crossovers { get; } = {"Aex","HRndX", "HProX", "HGreX"};
    public string[] Eliminations { get; } = {"Elitism"};
    public string[] Mutations { get; } = {"Rsm"};
    public string[] Others { get; } = { };
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
        switch (OperatorType)
        {
            case OperatorTypes.Selection: return Selections;
            case OperatorTypes.Crossover: return Crossovers;
            case OperatorTypes.Elimination: return Eliminations;
            case OperatorTypes.Mutation: return Mutations;
            default: return Others;
        }
    }
}