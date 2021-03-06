using AbstractionProvider.Configuration;
using AbstractionProvider.Log;
using AbstractionProvider.PopulationModels;

namespace GeneticOptimization.Log;

public class Logger : ILogger
{
    private IList<LogFormat> logs;
    private DateTime _startTime;
    private DateTime _endTime;
    private IConfiguration _configuration;
    private string logDirectory;
    public double[] BestCostHistory { get; set; }
    public double[] AvgCostHistory { get; set; }
    public double[] MedianCostHistory { get; set; }
    public double[] WorstCostHistory { get; set; }
    public IPopulationModel BestModel { get; set; }

    public Logger(IConfiguration configuration, string resultName)
    {
        _configuration = configuration;
        BestCostHistory = new double[_configuration.MaxIterations + 1];
        AvgCostHistory = new double[_configuration.MaxIterations + 1];
        MedianCostHistory = new double[_configuration.MaxIterations + 1];
        WorstCostHistory = new double[_configuration.MaxIterations + 1];
        logs = new List<LogFormat>();
        LogFormat = new LogFormat();
        
        
        logDirectory = $"Results/{resultName}/Logs";
    }

    private void AddToLog(LogFormat line)
    {
        logs.Add(line);
    }

    public LogFormat LogFormat { get; set; }

    public void NextEpoch()
    {
        BestCostHistory[LogFormat.Epoch] = LogFormat.BestCost;
        AvgCostHistory[LogFormat.Epoch] = LogFormat.AvgCost;
        MedianCostHistory[LogFormat.Epoch] = LogFormat.MedianCost;
        WorstCostHistory[LogFormat.Epoch] = LogFormat.WorstCost;
        AddToLog(LogFormat);
        LogFormat = new LogFormat();
    }

    public string[] GetLog()
    {
        var stringLog = logs.Select(x =>
            $"{x.Epoch};{x.WorstCost:0.##};{x.AvgCost:0.##};{x.MedianCost:0.##};{x.BestCost:0.##};{x.MutationCount};{x.CrossWithoutConflicts - x.RandomizedResolves};{x.ConflictResolves};{x.RandomizedResolves}");
        return stringLog.ToArray();
    }

    public void StartTimer()
    {
        _startTime = DateTime.Now;
    }

    public void StopTimer()
    {
        _endTime = DateTime.Now;
    }

    public void WriteLogToFile()
    {
        var log = GetLog();
        var logString = new List<string>();
        
        logString.Add("Selection;Crossover;Mutation;Elimination;ConflictResolver;RandomisedResolver;RandomisedResolveProb;EpochCount;MutationProb;ParentsPerOffspring;StartTime;EndTime;Duration");
        logString.Add(
            $"{_configuration.OperatorInformation[0].OperatorName};{_configuration.OperatorInformation[1].OperatorName};" +
            $"{_configuration.OperatorInformation[2].OperatorName};{_configuration.OperatorInformation[3].OperatorName};" +
            $"{_configuration.ConflictResolveMethod};{_configuration.RandomisedResolveMethod};" +
            $"{_configuration.RandomisedResolveProbability};{_configuration.MaxIterations};" +
            $"{_configuration.MutationProbability:0.##};{_configuration.ParentsPerOffspring};" +
            $"{_startTime};{_endTime};{_endTime - _startTime}");
        var bestModelString = "";
        for (int i = 0; i < BestModel.Body.Length - 1; i++)
        {
            bestModelString += $"{BestModel.Body[i]},";
        }
        bestModelString += $"{BestModel.Body[^1]}";
        
        logString.Add("Best model: " + bestModelString);
        logString.Add($"Cost: {BestModel.Cost}");
        
        
        logString.Add("Epoch;WorstCost;AvgCost;MedianCost;BestCost;MutationCount;CrossWithoutConflict;ConflictResolves;RandomizedResolves");
        foreach (var l in log)
        {
            logString.Add(l);
        }

        var res = Directory.CreateDirectory(logDirectory);
        var logPath = $"{logDirectory}/log{DateTime.Now:dd_MM-HH_mm_ss_fff}.csv";
        if (File.Exists(logPath))
        {
            return;
        }
        WriteFile(logPath, logString, 0);
        
    }

    private void WriteFile(string logPath, List<string> logString, int depth)
    {
        if (depth == 10) return;
        try
        {
            File.WriteAllLines(logPath, logString);
        }
        catch
        {
            Thread.Sleep(10);
            logPath = $"{logDirectory}/log{DateTime.Now:dd_MM-HH_mm_ss_fff}.csv";
            WriteFile(logPath, logString, depth + 1);
        }
    }
}