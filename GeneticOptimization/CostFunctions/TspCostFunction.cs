using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.CostFunctions;

public class TspCostFunction
{
    [CostFunction]
    public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix, IConfiguration configuration)
    {
        var cost = 0d;
        for (int i = 0; i < populationModel.Body.Length - 1; i++)
        {
            cost += costMatrix.Matrix[populationModel.Body[i]][populationModel.Body[i + 1]];
        }

        if (populationModel.Body[^1] != 0)
            cost += costMatrix.Matrix[populationModel.Body[^1]][0];
        
        return cost;
    }
}