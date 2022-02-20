using AbstractionProvider.Configuration;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;

namespace GeneticOptimization.Operators.ConflictResolvers;

public class RandomResolver : ConflictResolver
{
    public RandomResolver(ICostMatrix costMatrix, IConfiguration configuration) : base(costMatrix, configuration)
    {
        
    }
    public override void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints)
    {
        var random = Random.Shared;
        var randomIndex = random.Next(0, remainingPoints.Count);
        currentBody[index] = remainingPoints[randomIndex];
        remainingPoints.Remove(remainingPoints[randomIndex]);
    }
}