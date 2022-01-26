using System;
using System.Windows.Input;
using Avalonia.Controls;
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
        var window = new ResultView((string) parameter);
        window.Show();
        
    }

    public event EventHandler? CanExecuteChanged;
}