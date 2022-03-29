using System;
using System.Windows.Input;
using Avalonia.Controls;
using Runner.Models;
using Runner.Views;

namespace Runner.Commands;

public class OpenRunInfo : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var window = new ResultView((ResultFileModel)parameter);
        window.Title = "Results/" + ((ResultFileModel) parameter).FileName;
        window.Show();
        
    }

    public event EventHandler? CanExecuteChanged;
}