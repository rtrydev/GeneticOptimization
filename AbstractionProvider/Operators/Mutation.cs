using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Log;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Operators;

public abstract class Mutation : ILoggable, IPopulationOperator, IOperatorWithResult<Population<IPopulationModel>>
{
    protected IConfiguration _configuration;
    protected ILogger _logger;
    protected ICostMatrix _costMatrix;

    protected Mutation(IConfiguration configuration, ICostMatrix costMatrix)
    {
        _costMatrix = costMatrix;
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

    public void AttachLogger(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger GetLogger()
    {
        return _logger;
    }
}