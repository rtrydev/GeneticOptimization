using GeneticOptimization.Configuration;
using GeneticOptimization.CostFunctions;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationInitializers;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Algorithm;

public class GeneticAlgorithm : ILoggable
{
    private IList<IOperator> _operators;
    private IConfiguration _configuration;
    private ICostMatrix _costMatrix;
    private ILogger _logger;

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
        _logger.StartTimer();
        var operatorsCount = _operators.Count;
        var populationSize = _configuration.PopulationSize;
        var population = new Population<IPopulationModel>(populationSize, TspPopulationGenerator.GenerateOneModel, WarehouseCostFunction.CalculateCost, _costMatrix);
        foreach (var individual in population.PopulationArray)
        {
            individual.Cost = population.CostFunction(individual, _costMatrix);
        }

        foreach (var o in _operators)
        {
            if (o is ILoggable loggable)
            {
                loggable.AttachLogger(_logger);
            }
        }

        population.PopulationArray = population.PopulationArray.OrderBy(x => x.Cost).ToArray();
        IData lastData = population;


        var maxIterations = _configuration.MaxIterations;

        for (int j = 0; j < maxIterations; j++)
        {
            for (int i = 0; i < operatorsCount; i++)
            {
                var geneticOperator = _operators[i];
                geneticOperator.AttachData(lastData);

                if (geneticOperator is IPopulationOperator @operator)
                {
                    @operator.AttachPopulation(population);
                    geneticOperator.Run();
                    continue;
                }

                lastData = geneticOperator.Run();
            }
            var costArray = population.PopulationArray.Select(x => population.CostFunction(x, _costMatrix)).OrderBy(x => x).ToArray();
            _logger.LogFormat.Epoch = j + 1;
            _logger.LogFormat.BestCost = costArray.Min();
            _logger.LogFormat.AvgCost = costArray.Average();
            _logger.LogFormat.MedianCost = costArray[costArray.Length / 2];
            _logger.LogFormat.WorstCost = costArray.Max();
            _logger.NextEpoch();  
        }

        _logger.BestModel = population.PopulationArray[0];
        _logger.StopTimer();
              
        
    }

    public void AttachLogger(ILogger logger)
    {
        _logger = logger;
    }

    public ILogger GetLogger()
    {
        return _logger;
    }
}