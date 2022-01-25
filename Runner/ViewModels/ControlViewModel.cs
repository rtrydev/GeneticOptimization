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
    public ConsoleLogModel LogModel { get; set; }
    public ICommand RunDistances { get; set; }

    public ControlViewModel(IConfiguration parametersModel, ConsoleLogModel logModel)
    {
        ParametersModel = parametersModel;
        LogModel = logModel;

        RunDistances = new RunDistances(parametersModel, logModel);
    }
}