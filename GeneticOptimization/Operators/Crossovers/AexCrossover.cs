using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public class AexCrossover : Crossover
{
    public AexCrossover(IConfiguration configuration, IConflictResolver conflictResolver, IConflictResolver randomizedResolver, ICostMatrix costMatrix) 
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
            var parents = new Parents<IPopulationModel>(parentCount);
            for (int j = 0; j < parentCount; j++)
            {
                parents.ParentsArray[j] = Data.ParentsArray[random.Next(Data.Length)];
            }

            var body = new int[bodyLength];
            body[0] = parents.ParentsArray[0].Body[0];
            var lastPoint = body[0];
            body[^1] = body[0];
            for (int j = 1; j < body.Length - 1; j++)
            {
                var nextIndex = Array.IndexOf(parents.ParentsArray[j % parentCount].Body, lastPoint) + 1;
                lastPoint = body[j - 1];

                if (random.NextDouble() <= randomisedResolveProb)
                {
                    _randomizedResolver.ResolveConflict(body, j, availablePoints);
                    _logger.LogFormat.RandomizedResolves++;
                    continue;
                }
                
                if (nextIndex == body.Length - 1)
                {
                    _conflictResolver.ResolveConflict(body, j, availablePoints);
                    _logger.LogFormat.ConflictResolves++;
                    continue;
                }
                var nextPoint = parents.ParentsArray[j % parentCount].Body[nextIndex];
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