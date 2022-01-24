using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators;

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

    public void Run()
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
        

    }
    
    
}