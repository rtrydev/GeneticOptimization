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

    private OperatorViewModel _operatorViewModel;

    public LoadConfig(OperatorViewModel operatorViewModel)
    {
        _operatorViewModel = operatorViewModel;
    }

    public async void Execute(object? parameter)
    {
        var vm = parameter as ParametersViewModel;
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = false;
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            var jsonString = await File.ReadAllTextAsync(result[0]);
            var config = JsonConvert.DeserializeObject<TspConfiguration>(jsonString);
            vm.Configuration = config;
            _operatorViewModel.Configuration = config;
            _operatorViewModel.OperatorInformation =
                new ObservableCollection<OperatorInformation>(config.OperatorInformation);
            vm.ControlViewModel.ParametersModel = config;
            vm.ControlViewModel.ReloadCommand();
        }
    }

    public event EventHandler? CanExecuteChanged;
}