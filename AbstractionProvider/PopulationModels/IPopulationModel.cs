namespace AbstractionProvider.PopulationModels;

public interface IPopulationModel
{
    public int[] Body { get; }
    public double Cost { get; set; }

}