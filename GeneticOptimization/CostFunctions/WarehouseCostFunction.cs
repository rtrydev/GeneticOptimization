using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;
using GeneticOptimization.Data;

namespace GeneticOptimization.CostFunctions;

public class WarehouseCostFunction
{
    private static string _ordersPath = "";
        
    [CostFunction]
    public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix, IConfiguration configuration)
    {
        if(_ordersPath == "") _ordersPath = configuration.DataPath.Replace("mag", "orders").Replace(".mtrx", ".txt").Replace(".tsp", ".txt");
        Orders.LoadOrders(_ordersPath);
        var orders = new Orders();
        return orders.GetOrderPathLengthSum(populationModel, costMatrix);
    }
}