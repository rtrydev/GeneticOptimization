using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Selections;

public class ElitismSelection : Selection
{
    public ElitismSelection(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}

    public override Parents<IPopulationModel> Run()
    {
        var parentCount = _configuration.ParentsCount;
        var parents = new Parents<IPopulationModel>(parentCount);
        
        for (int i = 0; i < parentCount; i++)
        {
            parents.ParentsArray[i] = Data.PopulationArray[i];
        }

        return parents;
    }
}