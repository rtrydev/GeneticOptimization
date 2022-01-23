using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Mutations;

public class BasicMutation : Mutation
{
    public override Population<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var probability = _configuration.GetPropertyValue<double>("MutationProbability");
        var population = Population.PopulationArray;
        foreach (var individual in population)
        {
            if (random.NextDouble() <= probability)
            {
                individual.Body[random.Next(0, individual.Body.Length)] = random.Next(0, 50);
            }
        }

        Population.PopulationArray = population;
        return Population;
    }

    public BasicMutation(IConfiguration configuration) : base(configuration) {}
}