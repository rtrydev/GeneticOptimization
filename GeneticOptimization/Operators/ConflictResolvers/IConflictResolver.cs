using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.ConflictResolvers;

public interface IConflictResolver
{
    public void ResolveConflict(int[] currentBody,int index, IList<int> remainingPoints);
}