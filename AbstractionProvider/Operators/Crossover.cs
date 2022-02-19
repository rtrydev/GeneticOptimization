using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Log;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Operators;

public abstract class Crossover : ILoggable, IOperatorWithInput<Parents<IPopulationModel>>, IOperatorWithResult<Offsprings<IPopulationModel>>
{
    public Parents<IPopulationModel> Data { get; set; }
    protected IConfiguration _configuration;
    protected IConflictResolver _conflictResolver;
    protected ILogger _logger;
    protected IConflictResolver _randomizedResolver;
    protected ICostMatrix _costMatrix;

    protected Crossover(IConfiguration configuration, IConflictResolver conflictResolver, IConflictResolver randomizedResolver, ICostMatrix costMatrix)
    {
        _configuration = configuration;
        _conflictResolver = conflictResolver;
        _randomizedResolver = randomizedResolver;
        _costMatrix = costMatrix;
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