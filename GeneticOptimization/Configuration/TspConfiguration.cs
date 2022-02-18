using System.Text.Json.Serialization;
using GeneticOptimization.Operators;
using GeneticOptimization.Operators.ConflictResolvers;

namespace GeneticOptimization.Configuration;

public class TspConfiguration : IConfiguration
{
    [Ignored]
    public string DataPath { get; set; }
    public string LogPath => GetLogPath();
    [Ignored] public string CostFunction { get; set; } = "GeneticOptimization.CostFunctions.TspCostFunction";
    public int PopulationSize { get; set; } = 120;
    public int ParentsCount { get; set; } = 90;
    public int OffspringCount { get; set; } = 60;
    public int ParentsPerOffspring { get; set; } = 8;
    public int MaxIterations { get; set; } = 300;
    public double MutationProbability { get; set; } = 0.1d;
    [Ignored]
    public OperatorInformation[] OperatorInformation { get; set; } = new[]
    {
        new OperatorInformation(OperatorTypes.Selection, "Random"),
        new OperatorInformation(OperatorTypes.Crossover, "HProX"),
        new OperatorInformation(OperatorTypes.Elimination, "Elitism"),
        new OperatorInformation(OperatorTypes.Mutation, "Rsm")
    };

    public ConflictResolveMethod ConflictResolveMethod { get; set; } = ConflictResolveMethod.NearestNeighbor;
    public ConflictResolveMethod RandomisedResolveMethod { get; set; } = ConflictResolveMethod.NearestNeighbor;
    public double RandomisedResolveProbability { get; set; } = 0.0d;

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

    public void SetPropertyValue(string name, object? value)
    {
        var property = GetType().GetProperty(name);
        if (property is null) throw new ArgumentException();
        try
        {
            property.SetValue(this, value);
        } catch (Exception e){}
    }

    public PropertyWrapper[] GetProperties()
    {
        var properties = GetType().GetProperties();
        return properties.Where(x => x.CanWrite && x.GetCustomAttributes(typeof(Ignored), false).Length == 0).Select(x => new PropertyWrapper(this, x.Name, x.PropertyType)).ToArray();
    }
    public PropertyWrapper[] GetPropertiesReadOnly()
    {
        var properties = GetType().GetProperties();
        return properties.Where(x => x.CanWrite).Select(x => new PropertyWrapper(this, x.Name, x.PropertyType)).ToArray();
    }

    public string GetLogPath()
    {
        return $"Logs/log{DateTime.Now:dd_MM-HH_mm_ss}.csv";
    }
    [JsonIgnore]
    public PropertyWrapper[] Properties => GetProperties();

    [JsonIgnore] 
    public PropertyWrapper[] PropertiesRead => GetPropertiesReadOnly();
}

