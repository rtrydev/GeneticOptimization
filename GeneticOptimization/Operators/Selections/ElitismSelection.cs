using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.Selections;

public class ElitismSelection : Selection
{
    public ElitismSelection(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}

    public override Parents<IPopulationModel> Run()
    {
        var parentCount = _configuration.ParentsCount < _configuration.PopulationSize ? _configuration.ParentsCount : _configuration.PopulationSize;
        var parents = new Parents<IPopulationModel>(parentCount);

        for (int i = 0; i < parentCount; i++)
        {
            parents.ParentsArray[i] = Data.PopulationArray[i];
        }

        return parents;
    }
}