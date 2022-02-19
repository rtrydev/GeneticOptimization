using AbstractionProvider.Data;
using AbstractionProvider.PopulationModels;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.PopulationInitializers;

public class TspPopulationGenerator
{
    public static IPopulationModel GenerateOneModel(ICostMatrix costMatrix)
    {
        var random = Random.Shared;
        var body = new int[costMatrix.Matrix.Length];

        var internals = Enumerable.Range(1, body.Length - 1).OrderBy(x => random.Next()).ToArray();
        body[0] = 0;
        for (int i = 1; i < body.Length; i++)
        {
            body[i] = internals[i - 1];
        }

        return new TspPopulationModel(body, double.MaxValue);
    }
}