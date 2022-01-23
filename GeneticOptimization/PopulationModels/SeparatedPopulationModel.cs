namespace GeneticOptimization.PopulationModels;

public class SeparatedPopulationModel : IPopulationModel
{
    public int[] Body { get; }
    public Func<double> CostFunction { get; }
    public SeparatedPopulationModel(int[] body)
    {
        Body = body;
        CostFunction = () => Body.Sum();
    }
}