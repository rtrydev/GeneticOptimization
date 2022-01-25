using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Selections;

public class RandomSelection : Selection
{
    public RandomSelection(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}

    public override Parents<IPopulationModel> Run()
    {
        var parentCount = _configuration.ParentsCount;
        var parents = new Parents<IPopulationModel>(parentCount);
        var random = Random.Shared;
        
        for (int i = 0; i < parentCount; i++)
        {
            parents.ParentsArray[i] = Data.PopulationArray[random.Next(Data.Length)];
        }

        return parents;
    }
}