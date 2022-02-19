using AbstractionProvider.Data;
using AbstractionProvider.Operators;

namespace GeneticOptimization.Operators.ConflictResolvers;

public class RandomResolver : IConflictResolver
{
    public RandomResolver(ICostMatrix costMatrix)
    {
        
    }
    public void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints)
    {
        var random = Random.Shared;
        var randomIndex = random.Next(0, remainingPoints.Count);
        currentBody[index] = remainingPoints[randomIndex];
        remainingPoints.Remove(remainingPoints[randomIndex]);
    }
}