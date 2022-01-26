using GeneticOptimization.Configuration;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Algorithm;

public class GeneticAlgorithmResult
{
    public double[] BestCostHistory { get; set; }
    public double[] AvgCostHistory { get; set; }
    public double[] MedianCostHistory { get; set; }
    public double[] WorstCostHistory { get; set; }
    public IPopulationModel BestIndividual { get; set; }
    public IConfiguration Configuration { get; set; }
}