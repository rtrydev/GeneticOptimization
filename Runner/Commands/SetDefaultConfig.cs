using System;
using System.IO;
using System.Windows.Input;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;

namespace Runner.Commands;

public class SetDefaultConfig : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var parameters = (TspConfiguration) parameter;
        if(parameters is null) return;
        var json = JsonConvert.SerializeObject(parameters);
        File.WriteAllText("default-config.json", json);
    }

    public event EventHandler? CanExecuteChanged;
}