using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ReactiveUI.Fody.Helpers;

namespace Runner.ViewModels;

public class HistoryViewModel : ViewModelBase
{

    [Reactive] public string[] Files { get; set; }

    public HistoryViewModel()
    {
        Files = Directory.GetFiles("Logs");
        
    }
}