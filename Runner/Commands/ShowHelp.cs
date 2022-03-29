using System;
using System.Windows.Input;
using Avalonia.Controls;
using Runner.Views;

namespace Runner.Commands;

public class ShowHelp : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var window = new HelpView();
        window.Title = "Help Window";
        window.SizeToContent = SizeToContent.WidthAndHeight;
        window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        window.Show();
    }

    public event EventHandler? CanExecuteChanged;
}