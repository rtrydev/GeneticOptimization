using AbstractionProvider.Data;

namespace AbstractionProvider.Operators;

public interface IOperator
{
    public void AttachData(IData data);
    public IData Run();
}