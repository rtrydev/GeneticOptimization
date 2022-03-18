using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using GeneticOptimization.Data;

namespace GeneticOptimization.Operators.ConflictResolvers;

public class SingleProductFrequencyResolver : ConflictResolver 
{
    private int[] _warehousePointsByLocation;
    private int[] _productsByFrequency;

    public SingleProductFrequencyResolver(ICostMatrix costMatrix, IConfiguration configuration) : base(costMatrix, configuration)
    {
        var ordersPath = configuration.DataPath.Replace("mag", "orders").Replace(".mtrx", ".txt").Replace(".tsp", ".txt");
        Orders.LoadOrders(ordersPath);
        var orders = Orders.OrderPoints;
        var orderFrequencies = Orders.OrderFrequencies;
        _warehousePointsByLocation = Enumerable.Range(0, CostMatrix.Matrix.Length)
            .OrderBy(x => CostMatrix.Matrix[0][x]).ToArray();
        var productFrequencies = new int[CostMatrix.Matrix.Length];
        for (int i = 0; i < orders.Length; i++)
        {
            for (int j = 0; j < orders[i].Length; j++)
            {
                var product = orders[i][j];
                productFrequencies[product] += orderFrequencies[i];
            }
        }
        _productsByFrequency = Enumerable.Range(0, CostMatrix.Matrix.Length)
            .OrderByDescending(x => productFrequencies[x]).ToArray();

    }

    public override void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints){
        var bestCandidate = remainingPoints[0];
        var locationIndex = Array.IndexOf(_warehousePointsByLocation, index);
        var bestFit = Math.Abs(Array.IndexOf(_productsByFrequency, remainingPoints[0]) - locationIndex);

        for(int i = 0; i < remainingPoints.Count; i++) {
            var candidateIndex = Array.IndexOf(_productsByFrequency, remainingPoints[i]);
            var currentFit = Math.Abs(candidateIndex - locationIndex);
            if(currentFit < bestFit)
            {
                bestCandidate = remainingPoints[i];
                bestFit = currentFit;
            }
        }

        currentBody[index] = bestCandidate;
        remainingPoints.Remove(bestCandidate);

    }
}