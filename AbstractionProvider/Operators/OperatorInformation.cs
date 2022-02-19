using AbstractionProvider.Operators;

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
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "GeneticOptimization");
        var selections = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Selection))).Select(x => x.ToString().Split(".").Last()).ToArray();
        var crossovers = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Crossover))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        var eliminations = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Elimination))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        var mutations = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Mutation))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        
        
        
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