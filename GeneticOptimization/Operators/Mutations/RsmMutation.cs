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
        
        foreach (var individual in population)
        {
            if (random.NextDouble() <= probability)
            {
                _logger.LogFormat.MutationCount++;
                var j = random.Next(1, individual.Body.Length);
                var i = random.Next(1, j);
                Array.Reverse(individual.Body, i, j - i);
            }
        }
        
        Population.PopulationArray = population;
        return Population;
    }
}