using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Operators;

public enum ConflictResolveMethod
{
    NearestNeighbor,
    Random
}
public interface IConflictResolver
{
    public void ResolveConflict(int[] currentBody,int index, IList<int> remainingPoints);
}