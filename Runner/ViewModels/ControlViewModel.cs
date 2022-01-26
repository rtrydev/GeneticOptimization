using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GeneticOptimization.Configuration;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ControlViewModel : ViewModelBase
{
    public IConfiguration ParametersModel { get; set; }

    public ParametersViewModel ParametersViewModel { get; } 
    public ConsoleLogModel LogModel { get; set; }
    public ICommand RunDistances { get; set; }

    public ControlViewModel(IConfiguration parametersModel, ConsoleLogModel logModel, ParametersViewModel parametersViewModel)
    {
        ParametersModel = parametersModel;
        LogModel = logModel;
        ParametersViewModel = parametersViewModel;

        RunDistances = new RunOptimization(parametersModel, logModel);
    }
}