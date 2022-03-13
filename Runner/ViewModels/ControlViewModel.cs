using System;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class InstancesInfo
{
    public int Count { get; set; } = 1;
}
public class ControlViewModel : ViewModelBase
{
    public IConfiguration ParametersModel { get; set; }

    public ParametersViewModel ParametersViewModel { get; } 
    public HistoryViewModel HistoryViewModel { get; }
    public ConsoleLogModel LogModel { get; set; }
    public ICommand RunDistances { get; set; }

    [Reactive] public string ButtonText { get; set; } = "START";
    private InstancesInfo _instancesInfo;

    private void SetButtonText(string text)
    {
        this.ButtonText = text;
    }
    public int InstancesCount
    {
        get => _instancesInfo.Count;
        set
        {
            if(value < 0) return;
            _instancesInfo.Count = value;
        }
    }

    public ControlViewModel(IConfiguration parametersModel, ConsoleLogModel logModel, ParametersViewModel parametersViewModel, HistoryViewModel historyViewModel)
    {
        ParametersModel = parametersModel;
        LogModel = logModel;
        ParametersViewModel = parametersViewModel;
        HistoryViewModel = historyViewModel;
        _instancesInfo = new InstancesInfo();

        RunDistances = new RunOptimization(parametersModel, logModel, historyViewModel, _instancesInfo, SetButtonText);
    }
}