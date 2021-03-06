using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;

namespace GeneticOptimization.Operators.ConflictResolvers;

public class NearestNeighborResolver : ConflictResolver
{
    private ICostMatrix _costMatrix;
    private IConfiguration _configuration;

    public NearestNeighborResolver(ICostMatrix costMatrix, IConfiguration configuration) : base(costMatrix, configuration)
    {
        _costMatrix = costMatrix;
        _configuration = configuration;
    }
    public override void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints)
    {
        var lastPoint = currentBody[index - 1];
        var lowestCostPoint = GetLowestCostPoint(lastPoint, remainingPoints);
        currentBody[index] = lowestCostPoint;
        remainingPoints.Remove(lowestCostPoint);
    }

    private int GetLowestCostPoint(int currentPoint, IList<int> remainingPoints)
    {
        var lowestCostPoint = remainingPoints[0];
        var lowestCost = _costMatrix.Matrix[currentPoint][remainingPoints[0]];
        for (int i = 1; i < remainingPoints.Count; i++)
        {
            var currentCost = _costMatrix.Matrix[currentPoint][remainingPoints[i]];
            if (currentCost < lowestCost)
            {
                lowestCost = currentCost;
                lowestCostPoint = remainingPoints[i];
            }
        }

        return lowestCostPoint;
    }
}