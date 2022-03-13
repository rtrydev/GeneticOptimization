using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.LocalSearch;

public class LocalSearch3Opt : OtherOperator
{
    public LocalSearch3Opt(IConfiguration configuration, ICostMatrix costMatrix, double activationProbability) : base(configuration, costMatrix, activationProbability)
    {
    }

    public override Population<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var population = Population.PopulationArray;

        for (int k = 1; k < population.Length; k++)
        {
            if (random.NextDouble() <= _activationProbability)
            {
                Optimize3Opt(population[k]);
                population[k].Cost = Population.CostFunction(population[k], _costMatrix, _configuration);
            }
        }
        population = population.OrderBy(x => x.Cost).ToArray();
        Population.PopulationArray = population;
        return Population;
    }
    private void Optimize3Opt(IPopulationModel populationModel)
    {
        var objectOrder = populationModel.Body;
        int improvements;
        do
        {
            improvements = 0;
            for (int i = 1; i < objectOrder.Length - 3; i++)
            {
                for (int j = i + 1; j < objectOrder.Length - 2; j++)
                {
                    for (int k = j + 1; k < objectOrder.Length - 1; k++)
                    {
                        if (TryOrderImprovement(i, j, k, populationModel))
                        {
                            improvements++;
                        }
                    }
                    
                }
            }
        } while (improvements > 0);
    }
        
    private bool TryOrderImprovement(int firstId, int secondId, int thirdId, IPopulationModel model)
    {
        var cost = Population.CostFunction(model, _costMatrix, _configuration);
        
        
        // 2 1 3
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);
        if (Population.CostFunction(model, _costMatrix, _configuration) < cost)
            return true;
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);
        
        // 1 3 2
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        if (Population.CostFunction(model, _costMatrix, _configuration) < cost)
            return true;
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        
        // 3 2 1
        Array.Reverse(model.Body, firstId, thirdId - firstId + 1);
        if (Population.CostFunction(model, _costMatrix, _configuration) < cost)
            return true;
        Array.Reverse(model.Body, firstId, thirdId - firstId + 1);
        
        // 2 3 1
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        if (Population.CostFunction(model, _costMatrix, _configuration) < cost)
            return true;
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);
        
        // 3 1 2
        Array.Reverse(model.Body, firstId, thirdId - firstId + 1);
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        if (Population.CostFunction(model, _costMatrix, _configuration) < cost)
            return true;
        Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
        Array.Reverse(model.Body, firstId, thirdId - firstId + 1);
        // 1 2 3
        return false;

    }

}