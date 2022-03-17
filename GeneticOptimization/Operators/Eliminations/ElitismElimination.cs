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

        var toEliminate = Data.Length < populationSize ? Data.Length : populationSize;
        for (int i = 0; i < Data.OffspringsArray.Length; i++)
        {
            Data.OffspringsArray[i].Cost = Population.CostFunction(Data.OffspringsArray[i], _costMatrix, _configuration);
        }

        Data.OffspringsArray = Data.OffspringsArray.OrderBy(x => x.Cost).ToArray();
        var offspringIterator = 0;
        for (int i = 1; i < toEliminate; i++)
        {
            population[i] = Data.OffspringsArray[offspringIterator];
            population[i].Cost = Data.OffspringsArray[offspringIterator++].Cost;
        }

        population = population.OrderBy(x => x.Cost).ToArray();

        Population.PopulationArray = population;
        return Population;

    }

    public ElitismElimination(IConfiguration configuration, ICostMatrix costMatrix) : base(configuration, costMatrix) {}
}