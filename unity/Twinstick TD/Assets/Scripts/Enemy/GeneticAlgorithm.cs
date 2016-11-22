using UnityEngine;
using System.Collections;

public class GeneticAlgorithm : MonoBehaviour {
    //Public variables
    public int[][] currentpopulation;
    public int[] fitness;

    //Private variables
    private int maxpopulationsize;
    private int maxchromosomelength;
    private int[][] currentpopulation;
    private 

   

    //Generate the next generation
    private void nextgeneration()
    {
        currentpopulation
    } 

    //Add skill points to chromosomes
    private int[][] addpoints(int[][]population, int points)
    {
        int pointsspent = 0;

        for(int i = 0; i < population.Length; i++)
        {
            int addpoints =  
        }
    }

    //Generate random population
    private int[][] random_population(int populationsize, int maxpoints, int skills)
    {
        int[][] population = new int[populationsize][];
        for(int i = 0; i < populationsize; i++)
        {
            population[i] = random_chromosome(maxpoints);
        }

        return population;
    }

    //Generate random chromosome
    private int[] random_chromosome (int maxpoints, int skills)
    {
        int pointsspent = 0;
        int[] chromosome = new int[skills];

        //Assign random points to skills
        for(int i = 0; i < skills; i++ )
        {
            int points = Random.Range(0, maxpoints - pointsspent + 1);
            chromosome[i] = points;
            pointsspent += points;
        }

        //Shuffle skills
        for(int i =0; i < skills; i++)
        {
            int j = Random.Range(0, skills);
            int currentvalue = chromosome[i];
            int nextvalue = chromosome[j];
            chromosome[j] = currentvalue;
            chromosome[i] = nextvalue;
        }

        return chromosome;
    }

    // Shuffle int array
    // https://docs.unity3d.com/Manual/RandomNumbers.html
    private int[] shuffleintarray(int[] chromosome)
    {
        for (int i = 0; i < chromosome.Length; i++)
        {
            int temp = chromosome[i];
            int randomIndex = Random.Range(0, chromosome.Length);
            chromosome[i] = chromosome[randomIndex];
            chromosome[randomIndex] = temp;
        }
    }

    // Choosing based on probabilities : returns index
    // https://docs.unity3d.com/Manual/RandomNumbers.html
    private int Choose(float[] probs)
    {
        float total = 0;

        foreach (float elem in probs)
        {
            total += elem;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < probs.Length; i++)
        {
            if (randomPoint < probs[i])
            {
                return i;
            }
            else
            {
                randomPoint -= probs[i];
            }
        }
        return probs.Length - 1;
    }

}
