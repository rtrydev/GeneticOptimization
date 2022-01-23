using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

var config = new GeneticConfiguration();
var selection = new WheelOfFortune(config);
var crossover = new KPoint(config);
var elimination = new Elitism(config);
var mutation = new BasicMutation(config);

var algorithm = new Algorithm(config);

algorithm.AddPluggable(selection);
algorithm.AddPluggable(crossover);
algorithm.AddPluggable(elimination);
algorithm.AddPluggable(mutation);
algorithm.Run();

