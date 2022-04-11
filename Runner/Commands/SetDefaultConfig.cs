using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;

namespace Runner.Commands;

public class SetDefaultConfig : ICommand
{
    private Func<Task> onSet;
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public SetDefaultConfig(Func<Task> task)
    {
        onSet = task;
    }

    public async void Execute(object? parameter)
    {
        var parameters = (TspConfiguration) parameter;
        if(parameters is null) return;
        var json = JsonConvert.SerializeObject(parameters);
        await File.WriteAllTextAsync("default-config.json", json);
        _ = Task.Run(onSet);
    }

    public event EventHandler? CanExecuteChanged;
}