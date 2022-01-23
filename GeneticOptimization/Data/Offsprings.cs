namespace GeneticOptimization.Data;

public class Offsprings<T> : IData
{
    public T[] OffspringsArray { get; }

    public int Length => OffspringsArray.Length;
    
    public Offsprings(int size)
    {
        OffspringsArray = new T[size];
    }
}