using System.Text.Json.Serialization;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;

namespace GeneticOptimization.Configuration;

public class TspConfiguration : IConfiguration
{
    [Ignored]
    public string DataPath { get; set; }
    public string LogPath => GetLogPath();
    [Ignored] public string CostFunction { get; set; } = "TspCostFunction";
    public int PopulationSize { get; set; } = 120;
    public int ParentsCount { get; set; } = 90;
    public int OffspringCount { get; set; } = 60;
    public int ParentsPerOffspring { get; set; } = 8;
    public int MaxIterations { get; set; } = 300;
    public double MutationProbability { get; set; } = 0.1d;
    [Ignored]
    public OperatorInformation[] OperatorInformation { get; set; } = new[]
    {
        new OperatorInformation(OperatorTypes.Selection, "RandomSelection"),
        new OperatorInformation(OperatorTypes.Crossover, "HProXCrossover"),
        new OperatorInformation(OperatorTypes.Elimination, "ElitismElimination"),
        new OperatorInformation(OperatorTypes.Mutation, "RsmMutation"), 
        new OperatorInformation(OperatorTypes.Other, "LocalSearch2Opt")
    };
    [Ignored]
    public string ConflictResolveMethod { get; set; } = "RandomResolver";
    [Ignored]
    public string RandomisedResolveMethod { get; set; } = "NearestNeighborResolver";
    [Ignored]
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

