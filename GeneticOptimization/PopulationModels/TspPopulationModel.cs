namespace GeneticOptimization.PopulationModels;

public class TspPopulationModel : IPopulationModel
{
    public int[] Body { get; }
    public double Cost { get; set; }

    public TspPopulationModel(int[] body, double cost)
    {
        Body = body;
        Cost = cost;
    }
}