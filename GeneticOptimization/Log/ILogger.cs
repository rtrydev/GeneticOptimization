using GeneticOptimization.PopulationModels;

namespace GeneticOptimization.Log;

public interface ILogger
{
    public LogFormat LogFormat { get; set; }
    public void NextEpoch();
    public string[] GetLog();
    public void StartTimer();
    public void StopTimer();
    public void WriteLogToFile();
    public IPopulationModel BestModel { get; set; }
}