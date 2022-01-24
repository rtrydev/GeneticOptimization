using GeneticOptimization.Operators;
using GeneticOptimization.Operators.ConflictResolvers;

namespace GeneticOptimization.Configuration;

public class TspConfiguration : IConfiguration
{
    public int PopulationSize { get; } = 120;
    public int ParentsCount { get; } = 60;
    public int OffspringCount { get; } = 30;
    public int ParentsPerOffspring { get; } = 8;
    public int MaxIterations { get; } = 300;
    public double MutationProbability { get; } = 0.1d;

    public string CostMatrixPath { get; } = "/Users/rtry/be52.tsp";
    public string LogPath { get; } = "/Users/rtry/log.csv";

    public OperatorInformation[] OperatorInformation { get; } = new[]
    {
        new OperatorInformation(OperatorTypes.Selection, "Roulette"),
        new OperatorInformation(OperatorTypes.Crossover, "HProX"),
        new OperatorInformation(OperatorTypes.Elimination, "Elitism"),
        new OperatorInformation(OperatorTypes.Mutation, "Rsm")
    };

    public ConflictResolveMethod ConflictResolveMethod { get; } = ConflictResolveMethod.NearestNeighbor;
    public ConflictResolveMethod RandomisedResolveMethod { get; } = ConflictResolveMethod.NearestNeighbor;
    public double RandomisedResolveProbability { get; } = 0.2d;

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