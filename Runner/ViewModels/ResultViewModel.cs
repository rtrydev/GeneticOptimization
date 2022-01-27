using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators;
using GeneticOptimization.PopulationModels;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Runner.ViewModels;

public class EpochValue
{
    public int Epoch { get; set; }
    public double Value { get; set; }
}
public class ResultViewModel : ViewModelBase
{
    public GeneticAlgorithmResult<TspPopulationModel, TspConfiguration> Result { get; set; }
    public OperatorInformation[] Operators { get; set; }
    
    public Collection<EpochValue> BestCosts { get; set; }
    public Collection<EpochValue> AvgCosts { get; set; }
    public Collection<EpochValue> MedianCosts { get; set; }
    public Collection<EpochValue> WorstCosts { get; set; }
    public ResultViewModel(string data)
    {
        var jsonString = File.ReadAllText(data);
        Result = JsonConvert.DeserializeObject<GeneticAlgorithmResult<TspPopulationModel, TspConfiguration>>(jsonString);

        BestCosts = new Collection<EpochValue>();
        AvgCosts = new Collection<EpochValue>();
        WorstCosts = new Collection<EpochValue>();
        MedianCosts = new Collection<EpochValue>();
        for (int i = 0; i < Result.Configuration.MaxIterations; i++)
        {
            AvgCosts.Add(new EpochValue()
            {
                Value = Result.AvgCostHistory[i],
                Epoch = i + 1
            });
            MedianCosts.Add(new EpochValue()
            {
                Value = Result.MedianCostHistory[i],
                Epoch = i + 1
            });
            BestCosts.Add(new EpochValue()
            {
                Value = Result.BestCostHistory[i],
                Epoch = i + 1
            });
            WorstCosts.Add(new EpochValue()
            {
                Value = Result.WorstCostHistory[i],
                Epoch = i + 1
            });
        }
        Operators = Result.Configuration.OperatorInformation;
    }

    public ResultViewModel()
    {
        
    }
}