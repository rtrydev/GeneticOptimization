using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Mutations;

public class RsmMutation : Mutation
{
    public RsmMutation(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}

    public override Population<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var probability = _configuration.MutationProbability;
        var population = Population.PopulationArray;

        for (int k = 1; k < population.Length; k++)
        {
            if (random.NextDouble() <= probability)
            {
                _logger.LogFormat.MutationCount++;
                var j = random.Next(1, population[k].Body.Length);
                var i = random.Next(1, j);
                Array.Reverse(population[k].Body, i, j - i);
                population[k].Cost = Population.CostFunction(population[k], _costMatrix);
                
            }
        }
        
        Population.PopulationArray = population;
        return Population;
    }
}