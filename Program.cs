using GeneticAlgorithmImplementationInCSharp;

class Program
{
    static void Main(string[] args)
    {
        int populationSize = 50;
        int numOfGenerations = 100;
        double generationGap = 0.5;
        GeneticAlgorithm.SelectionMethod selectionMethod = GeneticAlgorithm.SelectionMethod.RouletteWheel;
        GeneticAlgorithm.CrossoverMethod crossoverMethod = GeneticAlgorithm.CrossoverMethod.SinglePoint;
        double crossoverProbability = 0.8;
        double mutationProbability = 0.1;
        GeneticAlgorithm.ChromosomeType chromosomeType = GeneticAlgorithm.ChromosomeType.SUTBased;
        int chromosomeSize = 10;
        int alleleBase = 0;
        int alleleRange = 10;

        GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(populationSize, numOfGenerations, generationGap,
                                                                  selectionMethod, crossoverMethod, crossoverProbability,
                                                                  mutationProbability, chromosomeType, chromosomeSize,
                                                                  alleleBase, alleleRange);

        geneticAlgorithm.RunGA();

        Console.WriteLine("Genetic Algorithm completed successfully!");

        Console.ReadKey();
    }
}