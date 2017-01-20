using UnityEngine;
using System.Collections;

public class Chromosome {

    private Gene[] chromosome;

    private int amountOption1 = 0;
    private int amountOption2 = 0;
    private int amountOption3 = 0;
    private int amountOption4 = 0;
    int geneamount;


    public Chromosome(int geneamount)
    {
        this.geneamount = geneamount;
        chromosome = new Gene[geneamount];

    }


    public void createRandomChromosome()
    {
        for(int i = 0; i<chromosome.Length; i++)
        {
            Gene gene = new Gene();
            chromosome[i] = gene;
        }
            for (int i = 0; i < chromosome.Length; i++)
        {
        }

    }

    public void fromArrayToGene(int[] geneArray)
    {
        for(int i =0; i<geneArray.Length; i++)
        {
            chromosome[i].defineGeneOption(geneArray[i]);
        }
    }

    public int[] getArrayFromIndex(int index)
    {
        
        int[] arrayFromIndex = new int[geneamount-index];
        for(int i = 0; i< arrayFromIndex.Length; i++)
        {
        
            arrayFromIndex[i] = chromosome[i + index].getOption();
        }
        return arrayFromIndex;
    }

    public int[] getArrayTillIndex(int index)
    {
        int[] arrayTillIndex = new int[index];
        for(int i = 0; i<index; i++)
        {
            arrayTillIndex[i] = chromosome[i].getOption();
        }

        return arrayTillIndex;
    }

    public void setVarriables()
    {
        for (int i = 0; i < chromosome.Length; i++)
        {
            int option = chromosome[i].getOption();
            if(option == 1)
            {
                this.amountOption1++;
            }else if(option == 2)
            {
                this.amountOption2++;
            }else if(option == 3)
            {
                this.amountOption3++;
            }else if(option ==4)
            {
                this.amountOption4++;
            }
        }
      }
    
    public void mutate()
    {
        int geneNumber = Random.Range(0, geneamount - 1);
        chromosome[geneNumber].randomGene();
    }

    public int getAmountOption1()
    {
        return this.amountOption1;
    }
    public int getAmountOption2()
    {
        return this.amountOption2;

    }
    public int getAmountOption3()
    {
        return this.amountOption3;
    }
    public int getAmountOption4()
    {
        return this.amountOption4;
    }

    public int getLength()
    {
        return this.chromosome.Length;
    }

   public bool equals(Chromosome Other)
    {
        if(this.amountOption1 == Other.getAmountOption1() && this.amountOption2 == Other.getAmountOption2() && this.amountOption3 == Other.amountOption3 && this.amountOption4 == Other.amountOption4){
            return true;
        }
        return false;
    }
}
