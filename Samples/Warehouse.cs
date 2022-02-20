using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Operators;
using NearestNeighbor;

namespace GeneticOptimization.CostFunctions
{

    public class Orders
    {
        private static int[][]? _orderPoints;
        private static int[] _orderFrequencies;
        private static bool _loadingOrders = false;

        public static int[][]? OrderPoints => _orderPoints;
        public static int[] OrderFrequencies => _orderFrequencies;

        public static void LoadOrders(string path)
        {
            if(_loadingOrders) return;
            if(_orderPoints != null) return;
            _loadingOrders = true;
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
            _loadingOrders = false;

            }

            public double GetOrderPathLengthSum(IPopulationModel model, ICostMatrix costMatrix)
            {
                while (_loadingOrders)
                {
                    Thread.Sleep(10);
                }
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

            private int[][]? TranslatePoints(IPopulationModel model)
            {
                if (_orderPoints is null) return null;
                
                
                var result = new int[_orderPoints.Length][];
                for (int i = 0; i < _orderPoints.Length; i++)
                {
                    result[i] = TranslateWithChromosome(_orderPoints[i], model.Body);
                }

                return result;
            }
            
            private int[] TranslateWithChromosome(int[] order, int[] chromosome)
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

    public class WarehouseCostFunction
    {
        private static string _ordersPath = "";
        
        [CostFunction]
        public static double CalculateCost(IPopulationModel populationModel, ICostMatrix costMatrix, IConfiguration configuration)
        {
            if(_ordersPath == "") _ordersPath = configuration.DataPath.Replace("mag", "orders").Replace(".mtrx", ".txt");
            Orders.LoadOrders(_ordersPath);
            var orders = new Orders();
            return orders.GetOrderPathLengthSum(populationModel, costMatrix);
        }
    }

    public class SingleProductFrequencyResolver : IConflictResolver
    {
        private ICostMatrix _costMatrix;
        private int[] _warehousePointsByLocation;
        private int[] _productsByFrequency;
        private IConfiguration _configuration;

        public SingleProductFrequencyResolver(ICostMatrix costMatrix, IConfiguration configuration) {
            _costMatrix = costMatrix;
            _configuration = configuration;
            var ordersPath = _configuration.DataPath.Replace("mag", "orders").Replace(".mtrx", ".txt");
            Orders.LoadOrders(ordersPath);
            var orders = Orders.OrderPoints;
            var orderFrequencies = Orders.OrderFrequencies;
            _warehousePointsByLocation = Enumerable.Range(0, _costMatrix.Matrix.Length)
                .OrderBy(x => _costMatrix.Matrix[0][x]).ToArray();
            var productFrequencies = new int[_costMatrix.Matrix.Length];
            for (int i = 0; i < orders.Length; i++)
            {
                for (int j = 0; j < orders[i].Length; j++)
                {
                    var product = orders[i][j];
                    productFrequencies[product] += orderFrequencies[i];
                }
            }
            _productsByFrequency = Enumerable.Range(0, _costMatrix.Matrix.Length)
                .OrderByDescending(x => productFrequencies[x]).ToArray();

        }

        public void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints){
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
}
