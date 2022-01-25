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

public class RunDistances : ICommand
{
    private IConfiguration _parametersModel;
    private ConsoleLogModel _logModel;

    public RunDistances(IConfiguration parametersModel, ConsoleLogModel logModel)
    {
        _parametersModel = parametersModel;
        _logModel = logModel;
    }
    
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        _logModel.AppendLog($"Started TSP on dataset");
        
        //if(_parametersModel.SelectedFiles is null) return;
        //if(_parametersModel.SelectedFiles.Length < 1) return;

        var config = new TspConfiguration();
        var optimizer = new GeneticOptimizer(_parametersModel);

        var result = 0d;
        await Task.Run(() => result = optimizer.Run());
        _logModel.AppendLog("Result: " + result.ToString("0.##"));

        // foreach (var dataset in _parametersModel.SelectedFiles)
        // {
        //     _parametersModel.DataPath = dataset;
        //     var datasetName = OperatingSystem.IsWindows()
        //         ? _parametersModel.DataPath.Split("\\")[^1]
        //         : _parametersModel.DataPath.Split("/")[^1];
        //     
        //     _logModel.AppendLog($"Started TSP on {datasetName} dataset");
        //     TSPResult result = null;
        //     _ = await Task.Run(async () => result = OptimizationWork.TSP(_parametersModel, CancellationToken.None));
        //     if (result is not null)
        //     {
        //         _logModel.AppendLog("Result: " + result.FinalFitness.ToString("0.##"));
        //
        //     }
        // }
    }


    public event EventHandler? CanExecuteChanged;
}