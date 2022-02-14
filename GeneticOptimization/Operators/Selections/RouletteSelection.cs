using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Selections;

public class RouletteSelection : Selection
{
    private readonly Random _random = Random.Shared;

    public RouletteSelection(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix){} 
    public override Parents<IPopulationModel> Run()
    {
        var dataLength = Data.Length;
        var cost = new double[dataLength];
        var parentCount = _configuration.ParentsCount;
        var parents = new Parents<IPopulationModel>(parentCount);

        for (int i = 0; i < dataLength; i++)
        {
            cost[i] = Data.CostFunction(Data.PopulationArray[i], _costMatrix);
        }

        var costSum = cost.Sum();
        var fitness = new double[cost.Length];
        for (int i = 0; i < fitness.Length; i++)
        {
            fitness[i] = costSum / cost[i];
        }

        var fitnessSum = fitness.Sum();
        
        for (int i = 0; i < parentCount; i++)
        {
            var currentSpin = _random.NextDouble() * fitnessSum;
            var sum = 0d;
            for (int j = 0; j < dataLength; j++)
            {
                sum += fitness[j];
                if (sum >= currentSpin)
                {
                    parents.ParentsArray[i] = Data.PopulationArray[j];
                    break;
                }
            }
        }

        return parents;
    }
}