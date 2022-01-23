using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Data;

public class Population<T> : IData where T : IPopulationModel 
{
    public T[] PopulationArray { get; set;  }
    public int Length => PopulationArray.Length;
    public Func<T, double> CostFunction { get; }
    
    public Population(int size, Func<T> initializationFunc, Func<T, double> costFunction)
    {
        PopulationArray = new T[size];
        for (int i = 0; i < size; i++)
        {
            PopulationArray[i] = initializationFunc();
        }

        CostFunction = costFunction;
    }
}