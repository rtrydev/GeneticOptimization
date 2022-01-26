using System;
using System.IO;
using System.Text.Json;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationModels;
using Newtonsoft.Json;

namespace Runner.ViewModels;

public class ResultViewModel : ViewModelBase
{
    public GeneticAlgorithmResult<TspPopulationModel, TspConfiguration> Result { get; set; }
    public OperatorInformation[] Operators { get; set; }
    public ResultViewModel(string data)
    {
        var jsonString = File.ReadAllText(data);
        Result = JsonConvert.DeserializeObject<GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>>(jsonString);
        Operators = Result.Configuration.OperatorInformation;
        Console.WriteLine("test");
    }

    public ResultViewModel()
    {
        
    }
}