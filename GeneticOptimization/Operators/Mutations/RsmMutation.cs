using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Mutations;

public class RsmMutation : Mutation
{
    public RsmMutation(IConfiguration configuration) : base(configuration) {}

    public override Population<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var probability = _configuration.GetPropertyValue<double>("MutationProbability");
        var population = Population.PopulationArray;

        for (int k = 1; k < population.Length; k++)
        {
            if (random.NextDouble() <= probability)
            {
                _logger.LogFormat.MutationCount++;
                var j = random.Next(1, population[k].Body.Length - 1);
                var i = random.Next(1, j);
                Array.Reverse(population[k].Body, i, j - i);
                population[k].Cost = Population.CostFunction(population[k]);
                
            }
        }
        
        Population.PopulationArray = population;
        return Population;
    }
}