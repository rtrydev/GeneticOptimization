using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Runner.Models;
using Runner.Views;

namespace Runner.Commands;

public class OpenRunInfo : ICommand
{
    private ObservableCollection<ResultFileModel> _files;

    public OpenRunInfo(ObservableCollection<ResultFileModel> files)
    {
        _files = files;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }
    
    public void Execute(object? parameter)
    {
        var value = (ResultFileModel)parameter;
        if (!value.TileColor.Equals(Brushes.Transparent))
        {
            var index = _files.IndexOf(value);

            var newValue = new ResultFileModel { FileName = value.FileName, TileColor = Brushes.Transparent };
            _files[index] = newValue;
        }

        var window = new ResultView(value);
        window.Title = "Results/" + value.FileName;
        
        window.Owner.Closed += (sender, args) => window.Close();
        window.Show();
        
    }

    public event EventHandler? CanExecuteChanged;
}