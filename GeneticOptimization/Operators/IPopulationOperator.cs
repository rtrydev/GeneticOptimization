using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators;

public interface IPopulationOperator : IOperator
{
    public Population<IPopulationModel> Population { get; set; }
    public void AttachPopulation(Population<IPopulationModel> population);
}