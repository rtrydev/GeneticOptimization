using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public class HProXCrossover : Crossover
{
    public HProXCrossover(IConfiguration configuration, IConflictResolver conflictResolver, IConflictResolver randomizedResolver, ICostMatrix costMatrix) 
        : base(configuration, conflictResolver, randomizedResolver, costMatrix) {}

    public override Offsprings<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var offspringCount = _configuration.GetPropertyValue<int>("OffspringCount");
        var parentCount = _configuration.GetPropertyValue<int>("ParentsPerOffspring");
        var randomisedResolveProb = _configuration.GetPropertyValue<double>("RandomisedResolveProbability");
        
        var bodyLength = Data.ParentsArray[0].Body.Length;

        var offsprings = new Offsprings<IPopulationModel>(offspringCount);

        for (int i = 0; i < offspringCount; i++)
        {
            var availablePoints = Enumerable.Range(1, bodyLength - 2).ToList();

            var body = new int[bodyLength];
            body[0] = Data.ParentsArray[0].Body[0];
            var lastPoint = body[0];
            body[^1] = body[0];
            
            for (int j = 1; j < bodyLength - 1; j++)
            {
                var feasibleParents = new List<IPopulationModel>(Data.ParentsArray);

                for (int k = 0; k < feasibleParents.Count; k++)
                {
                    var selectedParent = feasibleParents[k];
                    var indexOfCurrentPointInSelectedParent = Array.IndexOf(selectedParent.Body, lastPoint);
                    if (indexOfCurrentPointInSelectedParent >= bodyLength - 1 ||
                        body.Contains(selectedParent.Body[indexOfCurrentPointInSelectedParent + 1]))
                    {
                        feasibleParents.Remove(selectedParent);
                    }
                }

                var costs = new double[feasibleParents.Count];
                for (int k = 0; k < feasibleParents.Count; k++)
                {
                    costs[k] = _costMatrix.Matrix[lastPoint][
                        feasibleParents[k].Body[Array.IndexOf(feasibleParents[k].Body, lastPoint) + 1]];

                }
                
                
                var costMax = costs.Max();
                var fitness = costs.Select(x => costMax / x).ToArray();

                var fitnessSum = fitness.Sum();

                var nextIndex = -1;
                var nextPoint = -1;
                
                var currentSpin = random.NextDouble() * fitnessSum;
                var sum = 0d;
                for (int l = 0; l < fitness.Length; l++)
                {
                    sum += fitness[l];
                    if (sum >= currentSpin)
                    {
                        nextIndex = Array.IndexOf(feasibleParents[l].Body, lastPoint) + 1;
                        nextPoint = feasibleParents[l].Body[nextIndex];
                        break;
                    }
                }

                if (random.NextDouble() <= randomisedResolveProb)
                {
                    _randomizedResolver.ResolveConflict(body, j, availablePoints);
                    _logger.LogFormat.RandomizedResolves++;
                    continue;
                }
                
                if (nextIndex == body.Length - 1 || nextIndex == -1)
                {
                    _conflictResolver.ResolveConflict(body, j, availablePoints);
                    _logger.LogFormat.ConflictResolves++;
                    continue;
                }
                 
                if (body.Contains(nextPoint))
                {
                    _conflictResolver.ResolveConflict(body, j, availablePoints);
                    _logger.LogFormat.ConflictResolves++;
                }
                else
                {
                    body[j] = nextPoint;
                    availablePoints.Remove(nextPoint);
                }
            }

            offsprings.OffspringsArray[i] = new TspPopulationModel(body, double.MaxValue);
            
        }

        return offsprings;
    }
}