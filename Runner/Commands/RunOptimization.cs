using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GeneticOptimization;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.PopulationModels;
using ReactiveUI.Fody.Helpers;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Commands;

public class RunOptimization : ICommand
{
    private IConfiguration _parametersModel;
    private ConsoleLogModel _logModel;
    private HistoryViewModel _historyViewModel;
    private InstancesInfo _instancesInfo;
    private bool IsWorking;

    public RunOptimization(IConfiguration parametersModel, ConsoleLogModel logModel, HistoryViewModel historyViewModel, InstancesInfo instancesInfo)
    {
        _instancesInfo = instancesInfo;
        _parametersModel = parametersModel;
        _logModel = logModel;
        _historyViewModel = historyViewModel;
    }
    
    public bool CanExecute(object? parameter)
    {
        return !IsWorking;
    }

    public async void Execute(object? parameter)
    {
        var data = parameter as string[];
        if (data is null)
        {
            _logModel.AppendLog("You haven't selected any dataset yet!");
            return;
        }

        IsWorking = true;
        _ = Task.Run(() =>
        {
            for (int i = 0; i < data.Length; i++)
            {
                _logModel.AppendLog($"Started {_instancesInfo.Count} instances of TSP on dataset {data[i]}");

                var config = _parametersModel;
                config.DataPath = data[i];
                var optimizer = new GeneticOptimizer(config);

                if (_instancesInfo.Count == 0) return;
            
                var results = new GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>[_instancesInfo.Count];
                Parallel.For(0, _instancesInfo.Count, j =>
                {
                    results[j] = optimizer.Run();
                });
            

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
                string jsonString = JsonSerializer.Serialize(finalResult);
                var dataset = "";
                if (OperatingSystem.IsWindows())
                    dataset = finalResult.Configuration.DataPath.Split("\\")[^1];
                else dataset = finalResult.Configuration.DataPath.Split("/")[^1];
                dataset = string.Join("",dataset.Split(".").SkipLast(1).ToArray());
                var filename = $"{dataset}-{DateTime.Now:dd_MM-HH_mm_ss}.json";
                File.WriteAllText($"Results/{filename}", jsonString);
                _historyViewModel.RefreshFiles();
            }
            IsWorking = false;

        });
        
        
        
    }


    public event EventHandler? CanExecuteChanged;
}