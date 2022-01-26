using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GeneticOptimization;
using GeneticOptimization.Configuration;
using Runner.Models;
using Runner.ViewModels;

namespace Runner.Commands;

public class RunOptimization : ICommand
{
    private IConfiguration _parametersModel;
    private ConsoleLogModel _logModel;
    private HistoryViewModel _historyViewModel;

    public RunOptimization(IConfiguration parametersModel, ConsoleLogModel logModel, HistoryViewModel historyViewModel)
    {
        _parametersModel = parametersModel;
        _logModel = logModel;
        _historyViewModel = historyViewModel;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var data = parameter as string[];
        
        for (int i = 0; i < data.Length; i++)
        {
            _logModel.AppendLog($"Started TSP on dataset {data[i]}");

            var config = _parametersModel;
            config.DataPath = data[i];
            var optimizer = new GeneticOptimizer(config);

            var result = await Task.Run(() =>  optimizer.Run());
            _logModel.AppendLog("Result: " + result.BestIndividual.Cost.ToString("0.##"));
            _historyViewModel.RefreshFiles();
        }
        
    }


    public event EventHandler? CanExecuteChanged;
}