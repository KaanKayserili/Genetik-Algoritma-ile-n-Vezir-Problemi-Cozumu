using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
namespace GA_Vezir_Problemi
{


    class Program
    {
        static int[] fitness(List<int[]> population)
        {
            int[] fitness = new int[population.Count];
            for (int i = 0; i < population.Count; i++)
            {
                int penalty = 0;
                for (int j = 0; j < population[i].Length; j++)
                {
                    for (int k = j + 1; k < population[i].Length; k++)
                    {
                        if (population[i][j] == population[i][k])
                        {
                            penalty++;
                        }
                        else if (Math.Abs(population[i][j] - population[i][k]) == Math.Abs(j - k))
                        {
                            penalty++;
                        }
                    }
                }
                fitness[i] = 28 - penalty;
            }
            return fitness;
        }

        static int[] selection(int[] fitness, int num_parents)
        {
            int[] parents = new int[num_parents];
            for (int i = 0; i < num_parents; i++)
            {
                int max_fitness_idx = Array.IndexOf(fitness, fitness.Max());
                parents[i] = max_fitness_idx;
                fitness[max_fitness_idx] = -1;
            }
            return parents;
        }

        static List<int[]> crossover(List<int[]> parents, int offspring_size)
        {
            List<int[]> offspring = new List<int[]>();
            Random rand = new Random();
            for (int i = 0; i < offspring_size; i++)
            {
                int[] parent1 = parents[rand.Next(parents.Count)];
                int[] parent2 = parents[rand.Next(parents.Count)];
                int[] child = new int[parent1.Length];
                int crossover_point = rand.Next(1, parent1.Length);
                Array.Copy(parent1, child, crossover_point);
                Array.Copy(parent2, crossover_point, child, crossover_point, parent2.Length - crossover_point);
                offspring.Add(child);
            }
            return offspring;
        }

        static List<int[]> mutation(List<int[]> offspring, double mutation_rate)
        {
            Random rand = new Random();
            for (int i = 0; i < offspring.Count; i++)
            {
                if (rand.NextDouble() < mutation_rate)
                {
                    int gene_idx1 = rand.Next(offspring[i].Length);
                    int gene_idx2 = rand.Next(offspring[i].Length);
                    int temp = offspring[i][gene_idx1];
                    offspring[i][gene_idx1] = offspring[i][gene_idx2];
                    offspring[i][gene_idx2] = temp;
                }
            }
            return offspring;
        }

        static int[] best_solution(List<int[]> population, int[] fitness)
        {
            int max_fitness_idx = Array.IndexOf(fitness, fitness.Max());
            return population[max_fitness_idx];
        }

        static void Main(string[] args)
        {
            int population_size = 10;
            double crossover_rate = 0.6;
            double mutation_rate = 0.01;
            int iterations = 10;

            // Initial population
            List<int[]> population = new List<int[]>();
            Random rand = new Random();
            for (int i = 0; i < population_size; i++)
            {
                int[] individual = Enumerable.Range(1, 8).OrderBy(x => rand.Next()).ToArray();
                population.Add(individual);
            }

            for (int iter = 0; iter < iterations; iter++)
            {
                int[] fitness_values = fitness(population);

                // Select parents
                int num_parents = (int)(population_size * crossover_rate);
                int[] parents_idx = selection(fitness_values, num_parents);
                List<int[]> parents = new List<int[]>();
                foreach (int idx in parents_idx)
                {
                    parents.Add(population[idx]);
                }

                // Crossover
                int offspring_size = population_size - num_parents;
                List<int[]> offspring = crossover(parents, offspring_size);

                // Mutation
                offspring = mutation(offspring, mutation_rate);

                // New population
                population.Clear();
                population.AddRange(parents);
                population.AddRange(offspring);

                // Best solution
                int[] best_sol = best_solution(population, fitness_values);
                Console.WriteLine("Iteration {0}: Best solution = [{1}]", iter + 1, string.Join(", ", best_sol));
            }
        }


    }
}