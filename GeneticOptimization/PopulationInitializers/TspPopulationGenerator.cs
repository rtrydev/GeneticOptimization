using GeneticOptimization.Data;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.PopulationInitializers;

public class TspPopulationGenerator
{
    public static IPopulationModel GenerateOneModel(ICostMatrix costMatrix)
    {
        var random = Random.Shared;
        var body = new int[costMatrix.Matrix.Length + 1];

        var internals = Enumerable.Range(1, body.Length - 2).OrderBy(x => random.Next()).ToArray();
        body[0] = 0;
        body[^1] = 0;
        for (int i = 1; i < body.Length - 1; i++)
        {
            body[i] = internals[i - 1];
        }

        return new TspPopulationModel(body, double.MaxValue);
    }
}