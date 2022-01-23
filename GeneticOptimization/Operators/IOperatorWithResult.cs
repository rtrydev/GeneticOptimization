namespace GeneticOptimization.Operators;

public interface IOperatorWithResult<out T> : IOperator
{
    public T Run();
}