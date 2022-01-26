using System.Text.Json;
using System.Text.Json.Serialization;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization;

public class GeneticOptimizer
{
    private IConfiguration _configuration;
    private ICostMatrix _costMatrix;
    
    public GeneticOptimizer(IConfiguration configuration)
    {
        _configuration = configuration;
        _costMatrix = new TspCostMatrix(configuration);
    }

    public GeneticAlgorithmResult<TspPopulationModel, TspConfiguration> Run()
    {
        var operators = new List<IOperator>();
        var logger = new Logger(_configuration);
        
        foreach (var operatorInfo in _configuration.OperatorInformation)
        {
            operators.Add(OperatorFactory.CreateOperator(operatorInfo, _configuration, _costMatrix));            
        }

        var geneticAlgorithm = new GeneticAlgorithm(_configuration);
        geneticAlgorithm.AttachLogger(logger);
        geneticAlgorithm.AddCostMatrix(_costMatrix);

        foreach (var geneticOperator in operators)
        {
            geneticAlgorithm.AddOperator(geneticOperator);
        }
        
        geneticAlgorithm.Run();
        logger.WriteLogToFile();
        var result = new GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>()
        {
            BestCostHistory = logger.BestCostHistory,
            AvgCostHistory = logger.AvgCostHistory,
            MedianCostHistory = logger.MedianCostHistory,
            WorstCostHistory = logger.WorstCostHistory,
            BestIndividual = logger.BestModel as TspPopulationModel,
            Configuration = _configuration as TspConfiguration
        };
        string jsonString = JsonSerializer.Serialize(result);
        var dataset = "";
        if (OperatingSystem.IsWindows())
            dataset = result.Configuration.DataPath.Split("\\")[^1];
        else dataset = result.Configuration.DataPath.Split("/")[^1];
        dataset = string.Join("",dataset.Split(".").SkipLast(1).ToArray());
        var filename = $"{dataset}-{DateTime.Now:dd_MM-HH_mm_ss}.json";
        File.WriteAllText($"Results/{filename}", jsonString);

        return result;

    }
    
    
}