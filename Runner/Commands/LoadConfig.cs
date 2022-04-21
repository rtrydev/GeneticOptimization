using System;
using System.Collections.ObjectModel;
using System.IO;
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


    public LoadConfig(OperatorViewModel operatorViewModel)
    {
    }

    public async void Execute(object? parameter)
    {
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = false;
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            var jsonString = await File.ReadAllTextAsync(result[0]);
            var config = JsonConvert.DeserializeObject<TspConfiguration>(jsonString);
            ConfigReloader.Reload(config);
        }
    }

    public event EventHandler? CanExecuteChanged;
}