using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

namespace GeneticOptimization.Configuration;

public class GeneticConfiguration : IConfiguration
{
    public int PopulationSize { get; } = 120;
    public int ParentsCount { get; } = 60;
    public int OffspringCount { get; } = 60;
    public int ParentsPerOffspring { get; } = 2;
    public int MaxIterations { get; } = 500;
    public double MutationProbability { get; } = 0.1d;

    public string CostMatrixPath { get; } = "/Users/rtry/be52.tsp";
    public string LogPath { get; } = "/Users/rtry/log.csv";

    public SelectionMethod SelectionMethod { get; } = SelectionMethod.Roulette;
    public CrossoverMethod CrossoverMethod { get; } = CrossoverMethod.Aex;
    public EliminationMethod EliminationMethod { get; } = EliminationMethod.Elitism;
    public MutationMethod MutationMethod { get; } = MutationMethod.Rsm;

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