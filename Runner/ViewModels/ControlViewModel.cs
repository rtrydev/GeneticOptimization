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
    [Reactive] public ICommand RunOptimization { get; set; }
    public ICommand ShowHelp { get; set; }

    [Reactive] public string ButtonText { get; set; } = "START";
    [Reactive] public int Progress { get; set; } = 0;
    private InstancesInfo _instancesInfo;
    private DateTime _startTime;
    private DateTime _lastEtaUpdate;
    [Reactive] public TimeSpan CurrentEta { get; set; }

    private void SetButtonText(string text)
    {
        this.ButtonText = text;
        if (text == "START")
        {
            CurrentEta = new TimeSpan();
        }
    }

    private void SetProgress(float value)
    {
        this.Progress = (int)(100 * value);
        var passedTime = DateTime.Now - _startTime;
        if (value != 0 && DateTime.Now - _lastEtaUpdate > new TimeSpan(0, 0, 1))
        {
            var eta = passedTime / value - passedTime;
            CurrentEta = new TimeSpan(eta.Hours, eta.Minutes, eta.Seconds);
            _lastEtaUpdate = DateTime.Now;
        }
    }

    private void SetStartTime(DateTime time)
    {
        _startTime = time;
        CurrentEta = TimeSpan.MaxValue;
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
    
    
    public void ReloadCommand()
    {
        RunOptimization = new RunOptimization(ParametersModel, LogModel, HistoryViewModel, _instancesInfo, SetButtonText, SetProgress, SetStartTime);
    }

    public ControlViewModel(IConfiguration parametersModel, ConsoleLogModel logModel, ParametersViewModel parametersViewModel, HistoryViewModel historyViewModel)
    {
        ParametersModel = parametersModel;
        LogModel = logModel;
        ParametersViewModel = parametersViewModel;
        HistoryViewModel = historyViewModel;
        _instancesInfo = new InstancesInfo();
        ShowHelp = new ShowHelp();

        RunOptimization = new RunOptimization(parametersModel, logModel, historyViewModel, _instancesInfo, SetButtonText, SetProgress, SetStartTime);
    }
}