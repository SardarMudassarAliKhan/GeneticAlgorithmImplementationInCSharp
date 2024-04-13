using System;
using System.Collections.Generic;

namespace GeneticAlgorithmImplementationInCSharp
{
    public class GeneticAlgorithm
    {
        private int populationSize;
        private int numOfGenerations;
        private double generationGap;
        private SelectionMethod selectionMethod;
        private CrossoverMethod crossoverMethod;
        private double crossoverProbability;
        private double mutationProbability;
        private ChromosomeType chromosomeType;
        private int chromosomeSize;
        private int alleleBase;
        private int alleleRange;
        private Random random;
        private List<Chromosome> population;
        private List<Chromosome> offspringPopulation;
        private int offspringPopulationSize;

        public GeneticAlgorithm(int populationSize, int numOfGenerations, double generationGap,
                                SelectionMethod selectionMethod, CrossoverMethod crossoverMethod,
                                double crossoverProbability, double mutationProbability,
                                ChromosomeType chromosomeType, int chromosomeSize,
                                int alleleBase, int alleleRange)
        {
            this.populationSize = populationSize;
            this.numOfGenerations = numOfGenerations;
            this.generationGap = generationGap;
            this.selectionMethod = selectionMethod;
            this.crossoverMethod = crossoverMethod;
            this.crossoverProbability = crossoverProbability;
            this.mutationProbability = mutationProbability;
            this.chromosomeType = chromosomeType;
            this.chromosomeSize = chromosomeSize;
            this.alleleBase = alleleBase;
            this.alleleRange = alleleRange;

            population = new List<Chromosome>();
            offspringPopulation = new List<Chromosome>();
            random = new Random();

            offspringPopulationSize = (int)(populationSize * generationGap);

            InitializePopulation();
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < populationSize; i++)
            {
                Chromosome chromosome = new Chromosome(chromosomeSize, alleleBase, alleleRange, random);
                population.Add(chromosome);
            }
        }

        public void RunGA()
        {
            for (int generation = 0; generation < numOfGenerations; generation++)
            {
                PerformSelection();
                Console.WriteLine($"Generation {generation + 1}: Selection done.");

                PerformCrossover();
                Console.WriteLine($"Generation {generation + 1}: Crossover done.");

                PerformMutation();
                Console.WriteLine($"Generation {generation + 1}: Mutation done.");

                UpdatePopulation();
                Console.WriteLine($"Generation {generation + 1}: Population updated.");
            }
        }

        private void PerformSelection()
        {
            double totalFitness = 0;
            foreach (Chromosome chromosome in population)
            {
                totalFitness += chromosome.CalculateFitness();
            }

            List<Chromosome> selectedChromosomes = new List<Chromosome>();

            while (selectedChromosomes.Count < offspringPopulationSize)
            {
                double randomFitness = random.NextDouble() * totalFitness;

                double cumulativeFitness = 0;
                foreach (Chromosome chromosome in population)
                {
                    cumulativeFitness += chromosome.CalculateFitness();
                    if (cumulativeFitness >= randomFitness)
                    {
                        selectedChromosomes.Add(chromosome);
                        break;
                    }
                }
            }

            population = selectedChromosomes;
        }

        private void PerformCrossover()
        {
            List<Chromosome> newOffspring = new List<Chromosome>();

            for (int i = 0; i < offspringPopulationSize; i += 2)
            {
                if (offspringPopulation.Count == 0)
                {
                    break;
                }

                int parent1Index = random.Next(offspringPopulation.Count);
                int parent2Index = random.Next(offspringPopulation.Count);

                Chromosome parent1 = offspringPopulation[parent1Index];
                Chromosome parent2 = offspringPopulation[parent2Index];

                int crossoverPoint = random.Next(1, chromosomeSize - 1);

                if (crossoverPoint >= 0 && crossoverPoint < parent1.Genes.Count && crossoverPoint < parent2.Genes.Count)
                {
                    List<int> childGenes1 = new List<int>(parent1.Genes.GetRange(0, crossoverPoint));
                    childGenes1.AddRange(parent2.Genes.GetRange(crossoverPoint, chromosomeSize - crossoverPoint));
                    Chromosome child1 = new Chromosome(childGenes1);

                    List<int> childGenes2 = new List<int>(parent2.Genes.GetRange(0, crossoverPoint));
                    childGenes2.AddRange(parent1.Genes.GetRange(crossoverPoint, chromosomeSize - crossoverPoint));
                    Chromosome child2 = new Chromosome(childGenes2);

                    newOffspring.Add(child1);
                    newOffspring.Add(child2);
                }
            }

            offspringPopulation = newOffspring;
        }

        private void PerformMutation()
        {
            foreach (Chromosome chromosome in offspringPopulation)
            {
                if (random.NextDouble() < mutationProbability)
                {
                    int mutateIndex = random.Next(chromosomeSize);
                    chromosome.Genes[mutateIndex] = random.Next(alleleBase - alleleRange, alleleBase + alleleRange + 1);
                }
            }
        }

        private void UpdatePopulation()
        {
            population.Sort((x, y) => y.CalculateFitness().CompareTo(x.CalculateFitness()));

            int minCount = Math.Min(populationSize, offspringPopulation.Count);
            for (int i = 0; i < minCount; i++)
            {
                population[populationSize - i - 1] = offspringPopulation[i];
            }
        }

        public class Chromosome
        {
            public List<int> Genes { get; private set; }

            public Chromosome(int size, int baseValue, int range, Random random)
            {
                Genes = new List<int>();
                for (int i = 0; i < size; i++)
                {
                    Genes.Add(random.Next(baseValue - range, baseValue + range + 1));
                }
            }

            public Chromosome(List<int> genes)
            {
                Genes = genes;
            }

            public double CalculateFitness()
            {
                double sum = 0;
                foreach (int gene in Genes)
                {
                    sum += gene;
                }

                return sum;
            }
        }

        public enum SelectionMethod
        {
            RouletteWheel,
        }

        public enum CrossoverMethod
        {
            SinglePoint,
        }

        public enum ChromosomeType
        {
            SUTBased,
        }
    }

}
