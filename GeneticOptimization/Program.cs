using GeneticOptimization.Algorithm;
using GeneticOptimization.Configuration;
using GeneticOptimization.Data;
using GeneticOptimization.Log;
using GeneticOptimization.Operators.ConflictResolvers;
using GeneticOptimization.Operators.Crossovers;
using GeneticOptimization.Operators.Eliminations;
using GeneticOptimization.Operators.Mutations;
using GeneticOptimization.Operators.Selections;

var config = new GeneticConfiguration();
var costMatrix = new TspCostMatrix(config);

var conflictResolver = new NearestNeighborResolver(costMatrix);
var randomisedResolver = new NearestNeighborResolver(costMatrix);
var logger = new Logger(config);

var selection = new ElitismSelection(config);
var crossover = new HProXCrossover(config, conflictResolver, randomisedResolver, costMatrix);
crossover.AttachLogger(logger);
var elimination = new ElitismElimination(config);
var mutation = new RsmMutation(config);
mutation.AttachLogger(logger);

var algorithm = new GeneticAlgorithm(config);

algorithm.AddOperator(selection);
algorithm.AddOperator(crossover);
algorithm.AddOperator(elimination);
algorithm.AddOperator(mutation);
algorithm.AddCostMatrix(costMatrix);
algorithm.AttachLogger(logger);
algorithm.Run();

logger.WriteLogToFile();

