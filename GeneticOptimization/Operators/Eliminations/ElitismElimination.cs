using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.Eliminations;

public class ElitismElimination : Elimination
{
    public override Population<IPopulationModel> Run()
    {
        var populationSize = Population.Length;

        var population = Population.PopulationArray;

        var toEliminate = Data.Length;
        var offspringIterator = 0;
        for (int i = populationSize - 1; i > populationSize - toEliminate; i--)
        {
            population[i] = Data.OffspringsArray[offspringIterator++];
            population[i].Cost = Population.CostFunction(population[i], _costMatrix, _configuration);
        }

        population = population.OrderBy(x => x.Cost).ToArray();

        Population.PopulationArray = population;
        return Population;

    }

    public ElitismElimination(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}
}