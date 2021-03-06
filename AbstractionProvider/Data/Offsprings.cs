namespace AbstractionProvider.Data;

public class Offsprings<T> : IData
{
    public T[] OffspringsArray { get; set; }

    public int Length => OffspringsArray.Length;
    
    public Offsprings(int size)
    {
        OffspringsArray = new T[size];
    }
}