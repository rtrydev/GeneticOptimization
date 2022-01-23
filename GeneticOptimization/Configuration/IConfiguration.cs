namespace GeneticOptimization.Configuration;

public interface IConfiguration
{
    public T GetPropertyValue<T>(string name);
}