using System;
using System.Linq;
using GeneticOptimization.Configuration;
using GeneticOptimization.CostFunctions;
using ReactiveUI.Fody.Helpers;

namespace Runner.ViewModels;

public class OperatorViewModel : ViewModelBase
{
    public IConfiguration Configuration { get; }

    public OperatorViewModel(IConfiguration configuration)
    {
        Configuration = configuration;
    }
}