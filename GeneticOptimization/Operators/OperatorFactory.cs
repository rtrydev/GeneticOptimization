using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

namespace GeneticOptimization.Operators;

public class OperatorFactory
{
    public static IOperator CreateOperator(OperatorInformation operatorInformation, IConfiguration configuration, ICostMatrix costMatrix)
    {
        if (operatorInformation.OperatorType == OperatorTypes.Crossover)
        {
            var conflictResolver = CreateConflictResolver(configuration, costMatrix);
            var randomisedResolver = CreateRandomisedResolver(configuration, costMatrix);
            switch (operatorInformation.OperatorName)
            {
                case "Aex":
                    return new AexCrossover(configuration, conflictResolver, randomisedResolver, costMatrix);
                case "HProX":
                    return new HProXCrossover(configuration, conflictResolver, randomisedResolver, costMatrix);
                case "HGreX":
                    return new HGreXCrossover(configuration, conflictResolver, randomisedResolver, costMatrix);
                case "HRndX":
                    return new HRndXCrossover(configuration, conflictResolver, randomisedResolver, costMatrix);
            }
        }
        
        if (operatorInformation.OperatorType == OperatorTypes.Selection)
        {
            switch (operatorInformation.OperatorName)
            {
                case "Random":
                    return new RandomSelection(configuration, costMatrix);
                case "Roulette":
                    return new RouletteSelection(configuration, costMatrix);
                case "Elitism":
                    return new ElitismSelection(configuration, costMatrix);
            }
        }

        if (operatorInformation.OperatorType == OperatorTypes.Mutation)
        {
            switch (operatorInformation.OperatorName)
            {
                case "Rsm": return new RsmMutation(configuration, costMatrix);
            }
        }

        if (operatorInformation.OperatorType == OperatorTypes.Elimination)
        {
            switch (operatorInformation.OperatorName)
            {
                case "Elitism": return new ElitismElimination(configuration, costMatrix);
            }
        }

        throw new ArgumentException();
    }

    public static IConflictResolver CreateConflictResolver(IConfiguration configuration, ICostMatrix costMatrix)
    {
        switch (configuration.ConflictResolveMethod)
        {
            case ConflictResolveMethod.Random: return new RandomResolver();
            case ConflictResolveMethod.NearestNeighbor: return new NearestNeighborResolver(costMatrix);
        }

        throw new ArgumentException();
    }
    
    public static IConflictResolver CreateRandomisedResolver(IConfiguration configuration, ICostMatrix costMatrix)
    {
        switch (configuration.RandomisedResolveMethod)
        {
            case ConflictResolveMethod.Random: return new RandomResolver();
            case ConflictResolveMethod.NearestNeighbor: return new NearestNeighborResolver(costMatrix);
        }

        throw new ArgumentException();
    }
    
}