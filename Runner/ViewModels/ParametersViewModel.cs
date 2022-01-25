using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI.Fody.Helpers;
using Runner.Commands;
using Runner.Models;

namespace Runner.ViewModels;

public class ParametersViewModel : ViewModelBase
{

    public List<string> Selections { get; set; } = new () {"Random", "Roulette", "Elitism"};
    public List<string> Crossovers { get; set; } = new() {"Aex", "HGreX", "HProX"};
    public List<string> Eliminations { get; set; } = new() {"Elitism"};
    public List<string> Mutations { get; set; } = new() {"Rsm"};
    public ICommand SelectData { get; set; }
    public IConfiguration Configuration { get; set; } = new TspConfiguration();
    [Reactive] public string SelectedFilesString { get; set; }
    
    public ParametersViewModel(IConfiguration model)
    {
        Configuration = model;
        SelectData = new SelectData(Configuration);
    }
    
    
}