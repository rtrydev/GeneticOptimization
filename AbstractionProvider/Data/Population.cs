using AbstractionProvider.Configuration;
using AbstractionProvider.PopulationModels;

namespace AbstractionProvider.Data;

public class Population<T> : IData where T : IPopulationModel 
{
    public T[] PopulationArray { get; set;  }
    public int Length => PopulationArray.Length;
    public Func<IPopulationModel, ICostMatrix, IConfiguration, double> CostFunction { get; }
    
    public Population(int size, Func<ICostMatrix, T> initializationFunc, Func<IPopulationModel, ICostMatrix, IConfiguration, double> costFunction, ICostMatrix costMatrix)
    {
        PopulationArray = new T[size];
        for (int i = 0; i < size; i++)
        {
            PopulationArray[i] = initializationFunc(costMatrix);
        }

        CostFunction = costFunction;
    }
}