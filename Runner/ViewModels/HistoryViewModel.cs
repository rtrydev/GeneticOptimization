using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls.Selection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Views;

namespace Runner.ViewModels;

public class HistoryViewModel : ViewModelBase
{

    [Reactive] public string[] Files { get; set; }

    private string _selection;
    public string Selection
    {
        get => _selection;
        set
        {
            _selection = value;
            OpenRunInfo.Execute(value);
        }
    }

    public ICommand OpenRunInfo { get; set; }
    
    public HistoryViewModel()
    {
        RefreshFiles();
        OpenRunInfo = new OpenRunInfo();
    }

    public void RefreshFiles()
    {
        Files = Directory.GetFiles("Logs");
    }
}