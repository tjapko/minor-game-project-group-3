using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GaneticAlgorithm {
   private  EnemyPopulation population;
   private Chromosome parentA;
   private Chromosome parentB;
    private int popsize;
    private float mutationRate = 0.1f;
	private int waveNumber;

    
	public EnemyPopulation RunGA (EnemyPopulation population, int SizePop, int waveNumber) {
		this.waveNumber = waveNumber;
        this.popsize = SizePop;
        this.population = population;
        EnemyPopulation newGenartion;
        population.CalculateFitness();
        parentA = SelectPartent();
        parentB = SelectPartent();
        while (!parentA.equals(parentB))
        {
            parentB = SelectPartent();
        }
        newGenartion = generateOffspring();
        newGenartion.AddAndShuffle(population);
        return newGenartion;
	}

    private Chromosome SelectPartent()
    {
        float rand = Random.value;
        float prop = 0;
          foreach(KeyValuePair<EnemyInheratedValues, bool> enemy in population.getList())
        {
            if (rand < (enemy.Key.getFitness() + prop))
            {
                return enemy.Key.getChromosome();
            }
            prop += enemy.Key.getFitness();
        }

        Chromosome test = new Chromosome(20);
        return test;
    }

    private EnemyPopulation generateOffspring()
    {
        EnemyPopulation offspring = new EnemyPopulation();
        for(int i = 0; i< popsize; i++)
        {
            EnemyInheratedValues newchild = newChild();
            if (Random.Range(0f,1f) < mutationRate)
            {
                newchild.mutate();
            }

            offspring.AddEnemy(newchild);
        }
        return offspring;
    }

   

    private EnemyInheratedValues newChild()
    {
		EnemyInheratedValues child = new EnemyInheratedValues(waveNumber);
        int length = parentA.getLength();
        int crossoverindex = Random.Range(0, length);
        int[] childarray = new int[parentA.getLength()];
        parentA.getArrayTillIndex(crossoverindex).CopyTo(childarray, 0);
        parentB.getArrayFromIndex(crossoverindex).CopyTo(childarray, crossoverindex);
               child.getChromosome().fromArrayToGene(childarray);
        child.ChromosomeToValues();
        return child;
    }

    
}
