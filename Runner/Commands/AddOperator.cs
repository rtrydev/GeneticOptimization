using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AbstractionProvider.Configuration;
using AbstractionProvider.Operators;

namespace Runner.Commands;

public class AddOperator : ICommand
{
    private ObservableCollection<OperatorInformation> _operatorInformation { get; set; }
    private IConfiguration _configuration;

    public AddOperator(ObservableCollection<OperatorInformation> operatorInformation, IConfiguration configuration)
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
        var operatorToAdd = parameter as string;
        var opr = new OperatorInformation(OperatorTypes.Other, operatorToAdd);
        _operatorInformation.Add(opr);
        _configuration.OperatorInformation = _operatorInformation.ToArray();

    }

    public event EventHandler? CanExecuteChanged;
}