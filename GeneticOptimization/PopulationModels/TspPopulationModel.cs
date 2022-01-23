namespace GeneticOptimization.PopulationModels;

public class TspPopulationModel : IPopulationModel
{
    public int[] Body { get; }

    public TspPopulationModel(int[] body)
    {
        Body = body;
    }
}