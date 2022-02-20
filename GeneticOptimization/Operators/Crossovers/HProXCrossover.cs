using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public class HProXCrossover : Crossover
{
    public HProXCrossover(IConfiguration configuration, IConflictResolver conflictResolver, IConflictResolver randomizedResolver, ICostMatrix costMatrix) 
        : base(configuration, conflictResolver, randomizedResolver, costMatrix) {}

    public override Offsprings<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var offspringCount = _configuration.OffspringCount;
        var parentCount = _configuration.ParentsPerOffspring;
        var randomisedResolveProb = _configuration.RandomisedResolveProbability;
        
        var bodyLength = Data.ParentsArray[0].Body.Length;

        var offsprings = new Offsprings<IPopulationModel>(offspringCount);

        for (int i = 0; i < offspringCount; i++)
        {
            var availablePoints = Enumerable.Range(1, bodyLength - 1).ToList();

            var body = new int[bodyLength];
            body[0] = Data.ParentsArray[0].Body[0];
            var lastPoint = body[0];
            
            for (int j = 1; j < bodyLength; j++)
            {
                var feasibleParents = new List<IPopulationModel>();

                for (int k = 0; k < parentCount; k++)
                {
                    feasibleParents.Add(Data.ParentsArray[random.Next(0, Data.Length)]);
                }
                
                lastPoint = body[j - 1];

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
                    var index = Array.IndexOf(feasibleParents[k].Body, lastPoint) + 1;
                    var next = index == bodyLength ? 0 : index;
                    costs[k] = _costMatrix.Matrix[lastPoint][
                        feasibleParents[k].Body[next]];

                }
                
                var costSum = costs.Sum();
                var fitness = new double[costs.Length];
                for (int k = 0; k < fitness.Length; k++)
                {
                    fitness[k] = costSum / costs[k];
                }

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
                        if(nextIndex == bodyLength)
                        {
                            nextIndex = -1;
                            break;
                        }
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
                
                if (nextIndex == -1)
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
                    _logger.LogFormat.CrossWithoutConflicts++;
                    body[j] = nextPoint;
                    availablePoints.Remove(nextPoint);
                }
            }

            offsprings.OffspringsArray[i] = new TspPopulationModel(body, double.MaxValue);
            
        }

        return offsprings;
    }
}