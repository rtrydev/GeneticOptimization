using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Operators;

public abstract class OtherOperator : IPopulationOperator, IOperatorWithResult<Population<IPopulationModel>>
{
    public Population<IPopulationModel> Population { get; set; }
    protected IConfiguration _configuration;
    protected ICostMatrix _costMatrix;
    protected double _activationProbability;
    private IData _data;

    protected OtherOperator(IConfiguration configuration, ICostMatrix costMatrix, double activationProbability)
    {
        _configuration = configuration;
        _costMatrix = costMatrix;
        _activationProbability = activationProbability;
    }
    
    public void AttachPopulation(Population<IPopulationModel> population)
    {
        Population = population;
    }

    public void AttachData(IData data)
    {
        _data = data;
    }

    public abstract Population<IPopulationModel> Run();

    IData IOperator.Run()
    {
        return Run();
    }
}