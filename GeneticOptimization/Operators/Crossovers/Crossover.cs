using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public abstract class Crossover : ILoggable, IOperatorWithInput<Parents<IPopulationModel>>, IOperatorWithResult<Offsprings<IPopulationModel>>
{
    public Parents<IPopulationModel> Data { get; set; }
    protected IConfiguration _configuration;
    protected IConflictResolver _conflictResolver;
    protected ILogger _logger;

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

    public void AttachLogger(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger GetLogger()
    {
        return _logger;
    }
}