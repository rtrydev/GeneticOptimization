using GeneticOptimization.Data;

namespace GeneticOptimization.Operators;

public interface IOperator
{
    public void AttachData(IData data);
    public IData Run();
}