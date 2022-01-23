namespace GeneticOptimization.Configuration;

public class GeneticConfiguration : IConfiguration
{
    public int PopulationSize { get; } = 100;
    public int ParentsCount { get; } = 30;
    public int OffspringCount { get; } = 40;
    public int ParentsPerOffspring { get; } = 2;
    public int MaxIterations { get; } = 100;
    public double MutationProbability { get; } = 0.05d;

    public string CostMatrixPath { get; } = "/Users/rtry/be52.tsp";
    
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