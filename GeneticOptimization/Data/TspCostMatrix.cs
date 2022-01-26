using GeneticOptimization.Configuration;
using GeneticOptimization.Files;

namespace GeneticOptimization.Data;

public class TspCostMatrix : ICostMatrix
{
    public double[][] Matrix { get; }

    public TspCostMatrix(IConfiguration configuration)
    {
        var fileReader = new FileReader();
        var filePath = configuration.DataPath;
        Matrix = fileReader.ReadDistancesMatrix(filePath);
        
    }
}