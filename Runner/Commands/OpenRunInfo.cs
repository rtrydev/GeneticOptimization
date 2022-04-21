using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
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
        
        window.Owner.Closed += (sender, args) => window.Close();
        window.Show();
        
    }

    public event EventHandler? CanExecuteChanged;
}