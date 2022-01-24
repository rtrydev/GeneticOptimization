namespace GeneticOptimization.Operators;

public class OperatorInformation
{
    public OperatorTypes OperatorType { get; }
    public string OperatorName { get; }

    public OperatorInformation(OperatorTypes type, string name)
    {
        OperatorName = name;
        OperatorType = type;
    }
}