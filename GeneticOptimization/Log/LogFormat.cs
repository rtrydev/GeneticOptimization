namespace GeneticOptimization.Log;

public class LogFormat
{
    public int Epoch { get; set; }
    public int RandomizedResolves { get; set; }
    public int ConflictResolves { get; set; }
    public double WorstCost { get; set; }
    public double MedianCost { get; set; }
    public double AvgCost { get; set; }
    public double BestCost { get; set; }
    public int MutationCount { get; set; }
    public int CrossWithoutConflicts { get; set; }
    
}