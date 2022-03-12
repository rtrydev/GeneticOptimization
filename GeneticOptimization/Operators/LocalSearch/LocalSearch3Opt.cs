using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.LocalSearch;

public class LocalSearch3Opt : OtherOperator
{
    public LocalSearch3Opt(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix)
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

        


        /*
        var a = basePath[firstId - 1];
        var b = basePath[firstId];
        var c = basePath[secondId - 1];
        var d = basePath[secondId];
        var e = basePath[thirdId - 1];
        var f = basePath[thirdId];

        var d0 = distances[a][b] + distances[c][d] + distances[e][f];
        var d1 = distances[a][c] + distances[b][d] + distances[e][f];
        var d2 = distances[a][b] + distances[c][e] + distances[d][f];
        var d3 = distances[a][d] + distances[e][b] + distances[c][f];
        var d4 = distances[f][b] + distances[c][d] + distances[e][a];

        if (d0 > d1)
        {
            
            return -d0 + d1;
        } 
        else if (d0 > d2)
        {
            return -d0 + d2;
        }
        else if (d0 > d4)
        {
            return -d0 + d4;
        }
        /*else if (d0 > d3)
        {
            Array.Reverse(model.Body, secondId, thirdId - secondId + 1);
            return -d0 + d3;
        }#1#

        return 0d;*/
    }

}