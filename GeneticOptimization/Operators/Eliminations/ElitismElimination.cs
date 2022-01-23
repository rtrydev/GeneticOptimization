using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Eliminations;

public class ElitismElimination : Elimination
{
    public override Population<IPopulationModel> Run()
    {
        var populationSize = Population.Length;

        var population = Population.PopulationArray.OrderBy(x => Population.CostFunction(x)).ToArray();

        var toEliminate = Data.Length;
        var offspringIterator = 0;
        for (int i = populationSize - 1; i > populationSize - toEliminate; i--)
        {
            population[i] = Data.OffspringsArray[offspringIterator++];
        }

        Population.PopulationArray = population;
        return Population;

    }

    public ElitismElimination(IConfiguration configuration) : base(configuration) {}
}