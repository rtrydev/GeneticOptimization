using System.Text.Json.Serialization;
using GeneticOptimization.Operators;
using GeneticOptimization.Operators.ConflictResolvers;

namespace GeneticOptimization.Configuration;

public interface IConfiguration
{ 
    public int PopulationSize { get; set; }
    public int ParentsCount { get; set; }
    public int OffspringCount { get; set; }
    public int ParentsPerOffspring { get; set; }
    public int MaxIterations { get; set; }
    public double MutationProbability { get; set; }
    [JsonIgnore]
    public OperatorInformation[] OperatorInformation { get; set; }
    public string DataPath { get; set; }
    public string LogPath { get; }
    public ConflictResolveMethod ConflictResolveMethod { get; set; }
    public ConflictResolveMethod RandomisedResolveMethod { get; set; }
    public double RandomisedResolveProbability { get; set; }
    public T GetPropertyValue<T>(string name);
    public void SetPropertyValue(string name, object? value);
    public PropertyWrapper[] GetProperties();
    public PropertyWrapper[] GetPropertiesReadOnly();
    [JsonIgnore]
    public PropertyWrapper[] Properties { get; }
    [JsonIgnore]
    public PropertyWrapper[] PropertiesRead { get; }
}