using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public abstract class Crossover : IOperatorWithInput<Parents<IPopulationModel>>, IOperatorWithResult<Offsprings<IPopulationModel>>
{
    public Parents<IPopulationModel> Data { get; set; }
    protected IConfiguration _configuration;
    protected IConflictResolver _conflictResolver;

    protected Crossover(IConfiguration configuration, IConflictResolver conflictResolver)
    {
        _configuration = configuration;
        _conflictResolver = conflictResolver;
    }

    public void AttachData(Parents<IPopulationModel> data)
    {
        Data = data;
    }

    public void AttachData(IData data)
    {
        if (data is Parents<IPopulationModel> parents)
        {
            AttachData(parents);
        }
    }

    public abstract Offsprings<IPopulationModel> Run();

    IData IOperator.Run()
    {
        return Run();
    }
}