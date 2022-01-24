using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Eliminations;

public enum EliminationMethod
{
    Elitism
}
public abstract class Elimination : IPopulationOperator, IOperatorWithInput<Offsprings<IPopulationModel>>, IOperatorWithResult<Population<IPopulationModel>>
{
    public Population<IPopulationModel> Population { get; set; }
    protected IConfiguration _configuration;

    protected Elimination(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void AttachPopulation(Population<IPopulationModel> population)
    {
        Population = population;
    }

    public Offsprings<IPopulationModel> Data { get; set; }
    public void AttachData(Offsprings<IPopulationModel> data)
    {
        Data = data;
    }

    public void AttachData(IData data)
    {
        if (data is Offsprings<IPopulationModel> offsprings)
        {
            AttachData(offsprings);
        }
    }

    public abstract Population<IPopulationModel> Run();

    IData IOperator.Run()
    {
        return Run();
    }
}