using System.Reflection;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Data;
using AbstractionProvider.Log;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;
using GeneticOptimization.PopulationInitializers;


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
        var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "GeneticOptimization");
        var method = assembly.GetTypes()
            .SelectMany(t => t.GetMethods())
            .FirstOrDefault(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0 &&
                        m.DeclaringType.ToString().EndsWith(_configuration.CostFunction));

        var files = new DirectoryInfo("Modules").GetFiles().Select(x => x.FullName).ToArray();

        foreach (var file in files)
        {
            if (method is null)
            {
                var dynamicAssembly = Assembly.LoadFile(file);
                var dynamicallyLoadedMethods = dynamicAssembly.GetTypes()
                    .SelectMany(t => t.GetMethods())
                    .Where(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0)
                    .ToArray();

                method = dynamicallyLoadedMethods.FirstOrDefault(m => m.GetCustomAttributes(typeof(CostFunction), false).Length > 0 &&
                                                                      m.DeclaringType.ToString().EndsWith(_configuration.CostFunction));
            }
            else break;
        }
        
        if(method is null) return;

        var delegateCostFunction = method.CreateDelegate<Func<IPopulationModel, ICostMatrix, IConfiguration, double>>();
        
        var population = new Population<IPopulationModel>(populationSize, TspPopulationGenerator.GenerateOneModel, delegateCostFunction, _costMatrix);
        foreach (var individual in population.PopulationArray)
        {
            individual.Cost = population.CostFunction(individual, _costMatrix, _configuration);
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
            var costArray = population.PopulationArray.Select(x => population.CostFunction(x, _costMatrix, _configuration)).OrderBy(x => x).ToArray();
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