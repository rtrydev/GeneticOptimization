using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Algorithm;

public class Algorithm
{
    private IList<IOperator> _operators;
    private IConfiguration _configuration;

    public Algorithm(IConfiguration configuration)
    {
        _configuration = configuration;
        _operators = new List<IOperator>();
    }

    private IOperator? GetLastPlugin()
    {
        if (_operators.Count == 0) return null;
        return _operators.Last();
    }
    
    public void AddPluggable(IOperator geneticOperator)
    {
        var lastPlugin = GetLastPlugin();

        if (lastPlugin is null)
        {
            _operators.Add(geneticOperator);
            return;
        }

        _operators.Add(geneticOperator);
    }

    public void Run()
    {
        var random = Random.Shared;
        var operatorsCount = _operators.Count;
        var populationSize = _configuration.GetPropertyValue<int>("PopulationSize");
        var population = new Population<IPopulationModel>(populationSize, () =>
        {
            var body = new int[10];
            for (int i = 0; i < body.Length; i++)
            {
                body[i] = random.Next(0, 50);
            }

            return new SeparatedPopulationModel(body);
        }, model =>
        {
            var cost = model.Body.Sum();
            return cost;
        });
        IData lastData = population;

        var fitness = population.PopulationArray.Select(x => population.CostFunction(x)).ToArray();
        Console.WriteLine(fitness.Min());

        var maxIterations = _configuration.GetPropertyValue<int>("MaxIterations");

        for (int j = 0; j < maxIterations; j++)
        {
            for (int i = 0; i < operatorsCount; i++)
            {
                var geneticOperator = _operators[i];
                geneticOperator.AttachData(lastData);

                if (geneticOperator is IPopulationOperator @operator)
                {
                    @operator.AttachPopulation(population);
                }

                lastData = geneticOperator.Run();
            }
        }
        
        fitness = population.PopulationArray.Select(x => population.CostFunction(x)).ToArray();
        Console.WriteLine(fitness.Min());
        
        
    }
    
}