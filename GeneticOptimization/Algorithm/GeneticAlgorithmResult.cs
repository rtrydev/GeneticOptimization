using AbstractionProvider.Configuration;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Algorithm;

public class GeneticAlgorithmResult<TModel, TConfig> where TConfig : IConfiguration where TModel : IPopulationModel
{
    public double[] BestCostHistory { get; set; }
    public double[] AvgCostHistory { get; set; }
    public double[] MedianCostHistory { get; set; }
    public double[] WorstCostHistory { get; set; }
    public TModel BestIndividual { get; set; }
    public TConfig Configuration { get; set; }
}