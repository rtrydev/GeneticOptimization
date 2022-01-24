using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.CostFunctions;

public class TspCostFunction
{
    public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix)
    {
        var cost = 0d;
        for (int i = 0; i < populationModel.Body.Length - 1; i++)
        {
            cost += costMatrix.Matrix[populationModel.Body[i]][populationModel.Body[i + 1]];
        }
        return cost;
    }
}