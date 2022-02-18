using GeneticOptimization.Data;

namespace GeneticOptimization.NearestNeighbor;

public class NearestNeighbor
{
    private ICostMatrix _costMatrix;
    
    public NearestNeighbor(ICostMatrix costMatrix)
    {
        _costMatrix = costMatrix;
    }

    public double GetShortestPathLength()
    {
        var points = Enumerable.Range(0, _costMatrix.Matrix.Length).ToList();
        var path = new int[_costMatrix.Matrix.Length + 1];
        path[0] = 0;
        points.Remove(path[0]);
        path[^1] = 0;

        for (int i = 1; i < path.Length - 1; i++)
        {
            path[i] = GetNextPoint(path[i - 1], points);
            points.Remove(path[i]);
        }

        var cost = 0d;
        for (int i = 0; i < path.Length - 1; i++)
        {
            cost += _costMatrix.Matrix[path[i]][path[i + 1]];
        }

        return cost;

    }
    
    public double GetShortestPathLength(int[] pointsToOrder)
    {
        var points = pointsToOrder.ToList();
        var path = new int[points.Count + 2];
        path[0] = 0;
        if (points.Contains(0)) points.Remove(0);
        path[^1] = 0;

        for (int i = 1; i < path.Length - 1; i++)
        {
            path[i] = GetNextPoint(path[i - 1], points);
            points.Remove(path[i]);
        }

        var cost = 0d;
        for (int i = 0; i < path.Length - 1; i++)
        {
            cost += _costMatrix.Matrix[path[i]][path[i + 1]];
        }

        return cost;

    }

    private int GetNextPoint(int currentPoint, IList<int> availablePoints)
    {
        var bestCost = _costMatrix.Matrix[currentPoint][availablePoints[0]];
        var bestPoint = availablePoints[0];

        for (int i = 0; i < availablePoints.Count; i++)
        {
            var currentCost = _costMatrix.Matrix[currentPoint][availablePoints[i]];
            if (currentCost < bestCost)
            {
                bestCost = currentCost;
                bestPoint = availablePoints[i];
            }
        }

        return bestPoint;
    }
}