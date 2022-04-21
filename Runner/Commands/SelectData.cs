using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Runner.ViewModels;

namespace Runner.Commands;

public class SelectData : ICommand
{
    public IConfiguration parameters;

    public SelectData(IConfiguration model)
    {
        parameters = model;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var vm = parameter as ParametersViewModel;
        var config = vm.Configuration;
        var fileDialog = new OpenFileDialog();
        fileDialog.AllowMultiple = true;
        fileDialog.Filters = new List<FileDialogFilter>()
            { new FileDialogFilter() { Extensions = new List<string>() { "tsp", "mtrx" } } };
        var app = Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
        var result = await fileDialog.ShowAsync(app?.MainWindow);
        if (result is not null && result.Length > 0)
        {
            vm.SelectedData = result;
            vm.SelectedFilesString = GetFilesString(result);
        }
    }
    
    private string GetFilesString(string[] distanceSelectedFiles)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var file in distanceSelectedFiles)
        {
            if (OperatingSystem.IsWindows())
            {
                sb.Append($"{file.Split("\\")[^1].Replace(".tsp", "")}, ");
            }
            else
            {
                sb.Append($"{file.Split("/")[^1].Replace(".tsp", "")}, ");
            }
        }

        return sb.ToString();
    }

    public event EventHandler? CanExecuteChanged;
}