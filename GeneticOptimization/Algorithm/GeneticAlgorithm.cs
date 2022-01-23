using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Algorithm;

public class GeneticAlgorithm
{
    private IList<IOperator> _operators;
    private IConfiguration _configuration;
    private ICostMatrix _costMatrix;

    public GeneticAlgorithm(IConfiguration configuration)
    {
        _configuration = configuration;
        _operators = new List<IOperator>();
    }

    private IOperator? GetLastOperator()
    {
        if (_operators.Count == 0) return null;
        return _operators.Last();
    }
    
    public void AddOperator(IOperator geneticOperator)
    {
        var lastOperator = GetLastOperator();

        if (lastOperator is null)
        {
            _operators.Add(geneticOperator);
            return;
        }

        _operators.Add(geneticOperator);
    }

    public void AddCostMatrix(ICostMatrix costMatrix)
    {
        _costMatrix = costMatrix;
    }

    public void Run()
    {
        var random = Random.Shared;
        var operatorsCount = _operators.Count;
        var populationSize = _configuration.GetPropertyValue<int>("PopulationSize");
        var population = new Population<IPopulationModel>(populationSize, () =>
        {
            var body = new int[_costMatrix.Matrix.Length + 1];

            var internals = Enumerable.Range(1, body.Length - 2).OrderBy(x => random.Next()).ToArray();
            body[0] = 0;
            body[^1] = 0;
            for (int i = 1; i < body.Length - 1; i++)
            {
                body[i] = internals[i - 1];
            }

            return new TspPopulationModel(body);
        }, model =>
        {
            var cost = 0d;
            for (int i = 0; i < model.Body.Length - 1; i++)
            {
                cost += _costMatrix.Matrix[model.Body[i]][model.Body[i + 1]];
            }
            return cost;
        });
        IData lastData = population;

        var costArray = population.PopulationArray.Select(x => population.CostFunction(x)).ToArray();
        Console.WriteLine(costArray.Min());

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
        
        costArray = population.PopulationArray.Select(x => population.CostFunction(x)).ToArray();
        Console.WriteLine(costArray.Min());
        
        
    }
    
}