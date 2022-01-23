using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Mutations;

public abstract class Mutation : IPopulationOperator, IOperatorWithResult<Population<IPopulationModel>>
{
    protected IConfiguration _configuration;

    protected Mutation(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void AttachData(IData data)
    {
        return;
    }

    public abstract Population<IPopulationModel> Run();

    IData IOperator.Run()
    {
        return Run();
    }

    public Population<IPopulationModel> Population { get; set; }
    public void AttachPopulation(Population<IPopulationModel> population)
    {
        Population = population;
    }
}