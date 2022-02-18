using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.CostFunctions;

public class WarehouseCostFunction
{
    private static class Orders
    {
        private static int[][]? _orderPoints;
        private static int[] _orderFrequencies;

        public static void LoadOrders(string path)
        {
            if(_orderPoints is not null) return;
            var data = File.ReadAllLines(path);
            
            var nonEmptyLines = 0;
            for (int i = 0; i < data.Length; i++)
                if (data[i].Trim().Length > 1)
                    nonEmptyLines++;
            
            var convertedData = new int[nonEmptyLines][];
            for (int i = 0; i < nonEmptyLines; i++)
            {
                var s = data[i].Split(new [] { " " }, StringSplitOptions.RemoveEmptyEntries);
                convertedData[i] = Array.ConvertAll(s, int.Parse);
            }

            _orderPoints = new int[convertedData.Length][];
            _orderFrequencies = new int[convertedData.Length];
            
            for (int i = 0; i < convertedData.Length; i++)
            {
                _orderPoints[i] = new int[convertedData[i].Length - 1];
                for (int j = 0; j < _orderPoints[i].Length; j++)
                {
                    _orderPoints[i][j] = convertedData[i][j];
                }

                _orderFrequencies[i] = convertedData[i][^1];
            }

        }

        public static double GetOrderPathLengthSum(IPopulationModel model, ICostMatrix costMatrix)
        {
            if (_orderPoints is null) return 0d;

            var nn = new NearestNeighbor.NearestNeighbor(costMatrix);
            var translatedPoints = TranslatePoints(model);
            var cost = 0d;
            for (int i = 0; i < translatedPoints.Length; i++)
            {
                cost += nn.GetShortestPathLength(translatedPoints[i]) * _orderFrequencies[i];
            }
            
            return cost;
        }

        private static int[][]? TranslatePoints(IPopulationModel model)
        {
            if (_orderPoints is null) return null;
            
            var result = new int[_orderPoints.Length][];
            for (int i = 0; i < _orderPoints.Length; i++)
            {
                result[i] = TranslateWithChromosome(_orderPoints[i], model.Body);
            }

            return result;
        }
        
        private static int[] TranslateWithChromosome(int[] order, int[] chromosome)
        {
            int[] result = new int[order.Length];
            for (int i = 0; i < order.Length; i++)
            for (int c = 0; c < chromosome.Length; c++)
                if (order[i] == chromosome[c])
                {
                    result[i] = c;
                    break;
                }
            return result;
        }

    }
    
    public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix)
    {
        Orders.LoadOrders("/home/rtry/Projects/GeneticOptimization/Datasets/Warehouse/orders23b.txt");
        return Orders.GetOrderPathLengthSum(populationModel, costMatrix);
    }
}