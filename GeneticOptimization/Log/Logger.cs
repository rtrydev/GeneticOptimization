using GeneticOptimization.Configuration;
using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Log;

public class Logger : ILogger
{
    private IList<LogFormat> logs;
    private DateTime _startTime;
    private DateTime _endTime;
    private IConfiguration _configuration;
    public IPopulationModel BestModel { get; set; }

    public Logger(IConfiguration configuration)
    {
        _configuration = configuration;
        logs = new List<LogFormat>();
        LogFormat = new LogFormat();
    }

    private void AddToLog(LogFormat line)
    {
        logs.Add(line);
    }

    public LogFormat LogFormat { get; set; }

    public void NextEpoch()
    {
        AddToLog(LogFormat);
        LogFormat = new LogFormat();
    }

    public string[] GetLog()
    {
        var stringLog = logs.Select(x =>
            $"{x.Epoch};{x.WorstCost:0.##};{x.AvgCost:0.##};{x.MedianCost:0.##};{x.BestCost:0.##};{x.MutationCount};{x.ConflictResolves};{x.RandomizedResolves}");
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
        
        logString.Add("Selection;Crossover;Mutation;Elimination;ConflictResolver;RandomisedResolver;RandomisedResolveProb;EpochCount;MutationProb;StartTime;EndTime;Duration");
        logString.Add(
            $"{_configuration.OperatorInformation[0].OperatorName};{_configuration.OperatorInformation[1].OperatorName};{_configuration.OperatorInformation[2].OperatorName};{_configuration.OperatorInformation[3].OperatorName};{_configuration.ConflictResolveMethod};{_configuration.RandomisedResolveMethod};{_configuration.RandomisedResolveProbability};{_configuration.MaxIterations};{_configuration.MutationProbability:0.##};{_startTime};{_endTime};{_endTime - _startTime}");
        var bestModelString = "";
        for (int i = 0; i < BestModel.Body.Length - 1; i++)
        {
            bestModelString += $"{BestModel.Body[i]},";
        }
        bestModelString += $"{BestModel.Body[^1]}";
        
        logString.Add("Best model: " + bestModelString);
        logString.Add($"Cost: {BestModel.Cost}");
        
        
        logString.Add("Epoch;WorstCost;AvgCost;MedianCost;BestCost;MutationCount;ConflictResolves;RandomizedResolves");
        foreach (var l in log)
        {
            logString.Add(l);
        }
        File.WriteAllLines(_configuration.LogPath, logString);
    }
}