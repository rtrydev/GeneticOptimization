using AbstractionProvider.Configuration;
using AbstractionProvider.Data;

namespace AbstractionProvider.Operators;

public abstract class ConflictResolver : IConflictResolver
{
    protected ICostMatrix CostMatrix { get; set; }
    protected IConfiguration Configuration { get; set; }

    public ConflictResolver(ICostMatrix costMatrix, IConfiguration configuration)
    {
        Configuration = configuration;
        CostMatrix = costMatrix;
    }
    public abstract void ResolveConflict(int[] currentBody, int index, IList<int> remainingPoints);
}