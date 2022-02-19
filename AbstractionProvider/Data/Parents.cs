namespace AbstractionProvider.Data;

public class Parents<T> : IData
{
    public T[] ParentsArray { get; }

    public int Length => ParentsArray.Length;
    
    public Parents(int size)
    {
        ParentsArray = new T[size];
    }
}