using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.LocalSearch;

public class LocalSearch2Opt : OtherOperator
{
    public LocalSearch2Opt(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix)
    {
    }

    public override Population<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var probability = _configuration.LocalSearchProbability;
        var population = Population.PopulationArray;

        for (int k = 1; k < population.Length; k++)
        {
            if (random.NextDouble() <= probability)
            {
                Optimize2Opt(population[k]);
                population[k].Cost = Population.CostFunction(population[k], _costMatrix, _configuration);
            }
        }
        population = population.OrderBy(x => x.Cost).ToArray();
        Population.PopulationArray = population;
        return Population;
    }
    private void Optimize2Opt(IPopulationModel populationModel)
    {
        var objectOrder = populationModel.Body;
        int improvements;
        do
        {
            improvements = 0;
            for (int i = 1; i < objectOrder.Length - 2; i++)
            {
                for (int j = i + 1; j < objectOrder.Length - 1; j++)
                {
                    if (TryOrderImprovement(i, j, populationModel))
                    {
                        improvements++;
                    }
                }
            }
        } while (improvements > 0);
    }
        
    private bool TryOrderImprovement(int firstId, int secondId, IPopulationModel model)
    {
        var sumBefore = Population.CostFunction(model, _costMatrix, _configuration);
            
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);

        var sumAfter = Population.CostFunction(model, _costMatrix, _configuration);
        if (sumAfter < sumBefore)
            return true;
            
        Array.Reverse(model.Body, firstId, secondId - firstId + 1);
        return false;
    }

    
}