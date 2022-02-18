﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Avalonia;
using Avalonia.Interactivity;
using GeneticOptimization.Configuration;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;

namespace Runner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public IConfiguration Configuration { get; }
        public ParametersViewModel ParametersViewModel { get; }
        public ControlViewModel ControlViewModel { get; }
        public LogViewModel LogViewModel { get; }
        public HistoryViewModel HistoryViewModel { get; }
        
        public AlgorithmViewModel AlgorithmViewModel { get; }
        
        public OperatorViewModel OperatorViewModel { get; }
        public MainWindowViewModel(IConfiguration configuration, ParametersViewModel parametersViewModel, ControlViewModel controlViewModel, LogViewModel logViewModel, OperatorViewModel operatorViewModel, HistoryViewModel historyViewModel, AlgorithmViewModel algorithmViewModel)
        {
            Configuration = configuration;
            LogViewModel = logViewModel;
            ParametersViewModel = parametersViewModel;
            ControlViewModel = controlViewModel;
            OperatorViewModel = operatorViewModel;
            HistoryViewModel = historyViewModel;
            AlgorithmViewModel = algorithmViewModel;
            ControlViewModel.ParametersModel = Configuration;
        }

        public MainWindowViewModel()
        {
            
        }
    }
}