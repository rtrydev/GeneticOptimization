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
        var config = _configuration as GeneticConfiguration;
        logString.Add(
            $"{config.SelectionMethod};{config.CrossoverMethod};{config.MutationMethod};{config.EliminationMethod};{config.ConflictResolveMethod};{config.RandomisedResolveMethod};{config.RandomisedResolveProbability};{config.MaxIterations};{config.MutationProbability:0.##};{_startTime};{_endTime};{_endTime - _startTime}");
        var bestModelString = "";
        for (int i = 0; i < BestModel.Body.Length; i++)
        {
            bestModelString += $"{BestModel.Body[i]},";
        }
        logString.Add("Best model: " + bestModelString);
        
        logString.Add("Epoch;WorstCost;AvgCost;MedianCost;BestCost;MutationCount;ConflictResolves;RandomizedResolves");
        foreach (var l in log)
        {
            logString.Add(l);
        }
        File.WriteAllLines(config.LogPath, logString);
    }
}