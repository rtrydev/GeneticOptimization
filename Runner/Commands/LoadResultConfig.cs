using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;

namespace Runner.Commands;

public class LoadResultConfig : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    private IConfiguration _config;
    private Func<Task> onLoaded;

    public LoadResultConfig(IConfiguration config, Func<Task> task)
    {
        _config = config;
        onLoaded = task;
    }

    public async void Execute(object? parameter)
    {
        ConfigReloader.Reload(_config);
        _ = Task.Run(onLoaded);
    }

    public event EventHandler? CanExecuteChanged;
}