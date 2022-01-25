
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators;
using GeneticOptimization.Operators.ConflictResolvers;

namespace Runner.Models;

public class ParametersModel : IConfiguration
{
    public string[] SelectedFiles { get; set; }
    public int PopulationSize { get; }
    public int ParentsCount { get; }
    public int OffspringCount { get; }
    public int ParentsPerOffspring { get; }
    public int MaxIterations { get; }
    public double MutationProbability { get; }
    public OperatorInformation[] OperatorInformation { get; }
    public string CostMatrixPath { get; }
    public string LogPath { get; }
    public ConflictResolveMethod ConflictResolveMethod { get; }
    public ConflictResolveMethod RandomisedResolveMethod { get; }
    public double RandomisedResolveProbability { get; }
    public T GetPropertyValue<T>(string name)
    {
        throw new System.NotImplementedException();
    }
}