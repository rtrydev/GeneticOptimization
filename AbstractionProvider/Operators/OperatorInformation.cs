using System.Reflection;
using AbstractionProvider.CostFunctions;
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
        selections = selections.Concat(GetDynamicOperators(typeof(Selection))).ToArray();
        
        var crossovers = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Crossover))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        crossovers = crossovers.Concat(GetDynamicOperators(typeof(Crossover))).ToArray();
        
        var eliminations = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Elimination))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        eliminations = eliminations.Concat(GetDynamicOperators(typeof(Elimination))).ToArray();
        
        var mutations = assembly.GetTypes()
            .Where(c => c.IsSubclassOf(typeof(Mutation))).ToArray().Select(x => x.ToString().Split(".").Last()).ToArray();
        mutations = mutations.Concat(GetDynamicOperators(typeof(Mutation))).ToArray();
        
        
        switch (OperatorType)
        {
            case OperatorTypes.Selection: return selections;
            case OperatorTypes.Crossover: return crossovers;
            case OperatorTypes.Elimination: return eliminations;
            case OperatorTypes.Mutation: return mutations;
            default: return null;
        }
    }

    private string[] GetDynamicOperators(Type baseType)
    {
        var files = new DirectoryInfo("Modules").GetFiles().Select(x => x.FullName).ToArray();

        var result = Array.Empty<string>();
        
        foreach (var file in files)
        {
            var dynamicAssembly = Assembly.LoadFile(file);
            var dynamicallyLoadedClasses = dynamicAssembly.GetTypes()
                .Where(m => m.IsSubclassOf(baseType)).ToArray().Select(x => x.ToString().Split(".").Last())
                .ToArray();

            result = result.Concat(dynamicallyLoadedClasses).ToArray();
        }

        return result;

    }
}