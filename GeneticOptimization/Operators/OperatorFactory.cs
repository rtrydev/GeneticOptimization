using System.Reflection;
using AbstractionProvider.Configuration;
using AbstractionProvider.CostFunctions;
using AbstractionProvider.Data;
using AbstractionProvider.Operators;
using CodeCompiler;

namespace GeneticOptimization.Operators;

public class OperatorFactory
{
    public static IOperator CreateOperator(OperatorInformation operatorInformation, IConfiguration configuration, ICostMatrix costMatrix)
    {
        if (operatorInformation.OperatorType == OperatorTypes.Crossover)
        {
            var crossover = ClassProvider.GetClass(operatorInformation.OperatorName, typeof(Crossover));
            
            var types = new Type[4];
            types[0] = typeof(IConfiguration);
            types[1] = typeof(IConflictResolver);
            types[2] = typeof(IConflictResolver);
            types[3] = typeof(ICostMatrix);
            var conflictResolver = CreateConflictResolver(configuration, costMatrix);
            var randomisedResolver = CreateRandomisedResolver(configuration, costMatrix);
            var constructor = crossover.GetConstructor(types);
            object[] parameters = {configuration, conflictResolver, randomisedResolver, costMatrix};
            
            return (IOperator) constructor.Invoke(parameters);
        }
        
        if (operatorInformation.OperatorType == OperatorTypes.Selection)
        {
            var selection = ClassProvider.GetClass(operatorInformation.OperatorName, typeof(Selection));
            
            var types = new Type[2];
            types[0] = typeof(IConfiguration);
            types[1] = typeof(ICostMatrix);
            var constructor = selection.GetConstructor(types);
            object[] parameters = {configuration, costMatrix};
            
            return (IOperator) constructor.Invoke(parameters);
            
        }

        if (operatorInformation.OperatorType == OperatorTypes.Mutation)
        {
            var mutation = ClassProvider.GetClass(operatorInformation.OperatorName, typeof(Mutation));
            
            var types = new Type[2];
            types[0] = typeof(IConfiguration);
            types[1] = typeof(ICostMatrix);
            var constructor = mutation.GetConstructor(types);
            object[] parameters = {configuration, costMatrix};
            
            return (IOperator) constructor.Invoke(parameters);
        }

        if (operatorInformation.OperatorType == OperatorTypes.Elimination)
        {
            var elimination = ClassProvider.GetClass(operatorInformation.OperatorName, typeof(Elimination));
            
            var types = new Type[2];
            types[0] = typeof(IConfiguration);
            types[1] = typeof(ICostMatrix);
            var constructor = elimination.GetConstructor(types);
            object[] parameters = {configuration, costMatrix};
            
            return (IOperator) constructor.Invoke(parameters);
        }

        if (operatorInformation.OperatorType == OperatorTypes.Other)
        {
            var geneticOperator = ClassProvider.GetClass(operatorInformation.OperatorName, typeof(OtherOperator));

            var types = new Type[3];
            types[0] = typeof(IConfiguration);
            types[1] = typeof(ICostMatrix);
            types[2] = typeof(double);
            var constructor = geneticOperator.GetConstructor(types);
            object[] parameters = {configuration, costMatrix, operatorInformation.ActivationProbability};
            
            return (IOperator) constructor.Invoke(parameters);

        }
        
        throw new ArgumentException();
    }

    public static IConflictResolver CreateConflictResolver(IConfiguration configuration, ICostMatrix costMatrix)
    {
        return GetResolver(configuration.ConflictResolveMethod, costMatrix, configuration);
    }
    
    public static IConflictResolver CreateRandomisedResolver(IConfiguration configuration, ICostMatrix costMatrix)
    {
        return GetResolver(configuration.RandomisedResolveMethod, costMatrix, configuration);
    }

    private static IConflictResolver GetResolver(string resolverName, ICostMatrix costMatrix, IConfiguration configuration)
    {
        var resolver = ClassProvider.GetClass(resolverName, typeof(ConflictResolver));
        
        var types = new Type[2];
        types[0] = typeof(ICostMatrix);
        types[1] = typeof(IConfiguration);
        var constructor = resolver.GetConstructor(types);
        object[] parameters = {costMatrix, configuration};
            
        return (IConflictResolver) constructor.Invoke(parameters);
    }

}