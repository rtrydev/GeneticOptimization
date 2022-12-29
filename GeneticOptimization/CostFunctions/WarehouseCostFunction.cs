using System.Numerics;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;
using GeneticOptimization.Data;

namespace GeneticOptimization.CostFunctions;

public class WarehouseCostFunction
{
    private static string _ordersPath = "";
    private static BigInteger _invocations = 0;
        
    [CostFunction]
    public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix, IConfiguration configuration)
    {
        var ordersPath = configuration.DataPath.Replace("mag", "orders").Replace(".mtrx", ".txt").Replace(".tsp", ".txt");
        
        if (_ordersPath != ordersPath || _ordersPath == "" || _invocations == 0)
        {
            _invocations = 0;

            _ordersPath = ordersPath;
            Orders.LoadOrders(_ordersPath);
        }

        _invocations++;
        
        var orders = new Orders();
        return orders.GetOrderPathLengthSum(populationModel, costMatrix);
    }
}