using System.Reflection;

namespace GeneticOptimization.Configuration;

public class GeneticConfiguration : IConfiguration
{
    public int PopulationSize { get; } = 100;
    public int ParentsCount { get; } = 20;
    public int OffspringCount { get; } = 30;
    public int ParentsPerOffspring { get; } = 2;
    public int MaxIterations { get; } = 100;
    public double MutationProbability { get; } = 0.1d;
    
    
    public T GetPropertyValue<T>(string name)
    {
        var property = GetType().GetProperty(name);
        if (property is null) throw new ArgumentException();
        var value = property.GetValue(this);
        if (value is T result)
        {
            return result;
        }

        throw new ArgumentException();
    }
}