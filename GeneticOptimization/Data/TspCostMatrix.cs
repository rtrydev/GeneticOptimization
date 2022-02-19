using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using GeneticOptimization.Files;

namespace GeneticOptimization.Data;

public class TspCostMatrix : ICostMatrix
{
    public double[][] Matrix { get; }

    public TspCostMatrix(IConfiguration configuration)
    {
        var fileReader = new FileReader();
        var filePath = configuration.DataPath;
        if (filePath.EndsWith(".mtrx"))
        {
            Matrix = fileReader.ReadDistancesMatrix(filePath);
        }

        if (filePath.EndsWith(".tsp"))
        {
            var matrixFile = filePath.Replace(".tsp", ".mtrx");
            if (!File.Exists(matrixFile))
            {
                Matrix = fileReader.ReadDistancesMatrix(filePath);
                FileWriter.WriteArray(matrixFile, Matrix);
            }
            else
            {
                Matrix = fileReader.ReadDistancesMatrix(matrixFile);
            }
        }

        if (filePath.EndsWith(".txt"))
        {
            var matrixFile = filePath.Replace(".txt", ".mtrx");
            if (!File.Exists(matrixFile))
            {
                Matrix = fileReader.ReadDistancesMatrix(filePath);
                FileWriter.WriteArray(matrixFile, Matrix);
            }
            else
            {
                Matrix = fileReader.ReadDistancesMatrix(matrixFile);
            }
        }

    }
}