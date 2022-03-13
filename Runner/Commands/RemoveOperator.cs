using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Runner.Commands;

public class RemoveOperator : ICommand
{
    private ObservableCollection<OperatorInformation> _operatorInformation { get; set; }
    private IConfiguration _configuration;

    public RemoveOperator(ObservableCollection<OperatorInformation> operatorInformation, IConfiguration configuration)
    {
        _operatorInformation = operatorInformation;
        _configuration = configuration;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        var element = parameter as OperatorInformation;
        if(element is null) return;
        var elInList = _operatorInformation.FirstOrDefault(x => x.OperatorName == element.OperatorName);
        if(elInList is null) return;
        _operatorInformation.Remove(elInList);
        _configuration.OperatorInformation = _operatorInformation.ToArray();
        Console.WriteLine();
    }

    public event EventHandler? CanExecuteChanged;
}