using AbstractionProvider.Data;

namespace AbstractionProvider.Operators;

public interface IOperatorWithInput<T> where T : IData
{
    public T Data { get; set; }
    public void AttachData(T data);
}