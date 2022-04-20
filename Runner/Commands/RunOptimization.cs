using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.PopulationModels;
using GeneticOptimization;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Log;
using Newtonsoft.Json;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Commands;

public class RunOptimization : ICommand
{
    private IConfiguration _parametersModel;
    private ConsoleLogModel _logModel;
    private HistoryViewModel _historyViewModel;
    private InstancesInfo _instancesInfo;
    private CancellationTokenSource _tokenSource;
    private CancellationToken _cancellationToken;
    private bool IsWorking;
    private Action<string> setButtonString;
    private Action<float> setProgress;

    public RunOptimization(IConfiguration parametersModel, ConsoleLogModel logModel, HistoryViewModel historyViewModel, InstancesInfo instancesInfo, Action<string> buttonFunc, Action<float> progressFunc)
    {
        setButtonString = buttonFunc;
        setProgress = progressFunc;
        _instancesInfo = instancesInfo;
        _parametersModel = parametersModel;
        _logModel = logModel;
        _historyViewModel = historyViewModel;
        _tokenSource = new CancellationTokenSource();
        _cancellationToken = _tokenSource.Token;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        if (IsWorking)
        {
            _tokenSource.Cancel();
            _logModel.AppendLog("Cancelled");
            IsWorking = false;
            setButtonString("START");
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
            _cancellationToken = _tokenSource.Token;
            return;
        }
        var data = parameter as string[];
        if (data is null)
        {
            _logModel.AppendLog("You haven't selected any dataset yet!");
            return;
        }

        IsWorking = true;
        setButtonString("STOP");
        _ = Task.Run(() =>
        {
            for (int i = 0; i < data.Length; i++)
            {
                _logModel.AppendLog($"Started {_instancesInfo.Count} instances on dataset {data[i]}");

                var config = _parametersModel;
                config.DataPath = data[i];
                var optimizer = new GeneticOptimizer(config);

                if (_instancesInfo.Count == 0) return;

                var maxProgress = _instancesInfo.Count * config.MaxIterations;
                var progressMeters = new ProgressMeter[_instancesInfo.Count];
                for (int a = 0; a < progressMeters.Length; a++)
                {
                    progressMeters[a] = new ProgressMeter();
                }
                _ = Task.Run(() =>
                {
                    while (IsWorking)
                    {
                        Thread.Sleep(25);
                        var currentProgress = progressMeters.Sum(x => x.GetCurrent()) / (float)maxProgress;
                        setProgress(currentProgress);
                    }
                });
            
                var results = new GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>[_instancesInfo.Count];
                var dataset = "";
                if (OperatingSystem.IsWindows())
                    dataset = config.DataPath.Split("\\")[^1];
                else dataset = config.DataPath.Split("/")[^1];
                dataset = string.Join("",dataset.Split(".").SkipLast(1).ToArray());
                var resultName = $"{dataset}-{DateTime.Now:dd_MM-HH_mm_ss}";
                
                
                Parallel.For(0, _instancesInfo.Count, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, j =>
                {
                    results[j] = optimizer.Run(_cancellationToken, resultName, progressMeters[j]);
                });
                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var finalResult = new GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>()
                {
                    Configuration = results[0].Configuration,
                    AvgCostHistory = Enumerable.Range(0, results[0].AvgCostHistory.Length)
                        .Select(x => results.Select(y => y.AvgCostHistory[x]).Average()).ToArray(),
                    BestCostHistory = Enumerable.Range(0, results[0].BestCostHistory.Length)
                        .Select(x => results.Select(y => y.BestCostHistory[x]).Average()).ToArray(),
                    MedianCostHistory = Enumerable.Range(0, results[0].MedianCostHistory.Length)
                        .Select(x => results.Select(y => y.MedianCostHistory[x]).Average()).ToArray(),
                    WorstCostHistory = Enumerable.Range(0, results[0].WorstCostHistory.Length)
                        .Select(x => results.Select(y => y.WorstCostHistory[x]).Average()).ToArray(),
                    BestIndividual = results.Select(x => x.BestIndividual).First(x => x.Cost == results.Min(y => y.BestIndividual.Cost))
                };

                _logModel.AppendLog("Result: " + finalResult.BestIndividual.Cost.ToString("0.##"));
                string jsonString = JsonConvert.SerializeObject(finalResult);
                
                var filename = "result.json";
                if (!Directory.Exists($"Results/{resultName}"))
                {
                    Directory.CreateDirectory($"Results/{resultName}");
                }
                File.WriteAllText($"Results/{resultName}/{filename}", jsonString);
                var tspFile = config.DataPath.Replace(".mtrx", ".tsp");
                File.Copy(tspFile, $"Results/{resultName}/data.tsp");
                
                var parameters = (TspConfiguration) config;
                parameters.DataPath = "";
                if(parameters is null) return;
                var json = JsonConvert.SerializeObject(parameters);
                File.WriteAllText($"Results/{resultName}/config.json", json);
                
                _historyViewModel.RefreshFiles();
            }
            IsWorking = false;
            setButtonString("START");
        });
        
        
        
    }


    public event EventHandler? CanExecuteChanged;
}