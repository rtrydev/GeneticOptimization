namespace GeneticOptimization.Data;

public class TspCostMatrix : ICostMatrix
{
    public double[][] Matrix { get; }

    public TspCostMatrix(double[][] matrix)
    {
        Matrix = matrix;
    }
}