using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            //_selection = value;
            OpenRunInfo.Execute(value);
        }
    }

    public ICommand OpenRunInfo { get; set; }
    
    public HistoryViewModel()
    {
        Directory.CreateDirectory("Results");
        RefreshFiles();
        OpenRunInfo = new OpenRunInfo();
    }

    public void RefreshFiles()
    {
        DirectoryInfo info = new DirectoryInfo("Results");
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
        Files = files.Select(x => x.Name).ToArray();
    }
}