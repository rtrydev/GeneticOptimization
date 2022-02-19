using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Operators;

public interface IPopulationOperator : IOperator
{
    public Population<IPopulationModel> Population { get; set; }
    public void AttachPopulation(Population<IPopulationModel> population);
}