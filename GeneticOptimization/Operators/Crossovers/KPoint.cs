using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Operators.Crossovers;

public class KPoint : Crossover
{
    public override Offsprings<IPopulationModel> Run()
    {
        var random = Random.Shared;
        var offspringCount = _configuration.GetPropertyValue<int>("OffspringCount");
        var offsprings = new Offsprings<IPopulationModel>(offspringCount);

        for (int i = 0; i < offspringCount; i++)
        {
            var parentCount = _configuration.GetPropertyValue<int>("ParentsPerOffspring");
            var parents = new Parents<IPopulationModel>(parentCount);
            for (int j = 0; j < parentCount; j++)
            {
                parents.ParentsArray[j] = Data.ParentsArray[random.Next(Data.Length)];
            }

            var body = new int[parents.ParentsArray[0].Body.Length];
            for (int j = 0; j < body.Length; j++)
            {
                body[j] = parents.ParentsArray[j % parentCount].Body[j];
            }

            offsprings.OffspringsArray[i] = new SeparatedPopulationModel(body);

        }

        return offsprings;

    }

    public KPoint(IConfiguration configuration) : base(configuration) {}
}