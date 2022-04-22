using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using GeneticOptimization.Configuration;
using Newtonsoft.Json;
using Runner.ViewModels;

namespace Runner.Commands;

public class LoadConfig : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    private Func<Task> onLoaded;
    private Func<Task> onFailed;

    public LoadConfig(Func<Task> task, Func<Task> fail)
    {
        onLoaded = task;
        onFailed = fail;
    }

    public async void Execute(object? parameter)
    {
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = false;
        fileDialog.Filters = new List<FileDialogFilter>()
            { new FileDialogFilter() { Extensions = new List<string>() { "json" }, Name = "JSON files"} };
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            var jsonString = await File.ReadAllTextAsync(result[0]);
            try
            {
                var config = JsonConvert.DeserializeObject<TspConfiguration>(jsonString);
                ConfigReloader.Reload(config);
                _ = Task.Run(onLoaded);
            }
            catch
            {
                _ = Task.Run(onFailed);
            }
            
        }
    }

    public event EventHandler? CanExecuteChanged;
}