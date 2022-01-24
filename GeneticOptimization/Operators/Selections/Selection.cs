using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Selections;

public abstract class Selection : IOperatorWithInput<Population<IPopulationModel>>, IOperatorWithResult<Parents<IPopulationModel>>
{
    public Population<IPopulationModel> Data { get; set; }

    protected IConfiguration _configuration;
    protected ICostMatrix _costMatrix;

    public Selection(IConfiguration configuration, ICostMatrix costMatrix)
    {
        _configuration = configuration;
        _costMatrix = costMatrix;
    }
    public void AttachData(Population<IPopulationModel> data)
    {
        Data = data;
    }

    public void AttachData(IData data)
    {
        if (data is Population<IPopulationModel> population)
        {
            AttachData(population);
        }
    }

    public abstract Parents<IPopulationModel> Run();

    IData IOperator.Run()
    {
        return Run();
    }
}