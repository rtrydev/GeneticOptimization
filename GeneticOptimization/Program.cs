using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Files;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

var config = new GeneticConfiguration();
var fileReader = new FileReader();
var costMatrix = new TspCostMatrix(fileReader.ReadDistancesMatrix("/Users/rtry/be52.tsp"));

var conflictResolver = new NearestNeighborResolver(costMatrix);

var selection = new WheelOfFortune(config);
var crossover = new Aex(config, conflictResolver);
var elimination = new Elitism(config);
var mutation = new RsmMutation(config);

var algorithm = new Algorithm(config);

algorithm.AddPluggable(selection);
algorithm.AddPluggable(crossover);
algorithm.AddPluggable(elimination);
algorithm.AddPluggable(mutation);
algorithm.AddCostMatrix(costMatrix);
algorithm.Run();

