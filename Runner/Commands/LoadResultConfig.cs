using System;
using System.IO;
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

    public LoadResultConfig(IConfiguration config)
    {
        _config = config;
    }

    public async void Execute(object? parameter)
    {
        ConfigReloader.Reload(_config);
    }

    public event EventHandler? CanExecuteChanged;
}