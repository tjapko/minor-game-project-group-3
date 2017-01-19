using UnityEngine;
using System.Collections;

public class Gene {
    private int[] gene;
    private int option;

    public Gene()
    {
        gene = new int[2];
    }

    public void defineGeneOption(int Option)
    {
        this.option = Option;
        optiontogene();
    }

    public void defineGeneBit(int first, int second)
    {
        gene[0] = first;
        gene[1] = second;
        genetooption();
    }

    public void randomGene()
    {
        this.option = Random.Range(1, 5);
        optiontogene();
    }

    private void optiontogene()
    {
        if(this.option == 1)
        {
            gene[0] = 0;
            gene[1] = 0;
        }else if(this.option == 2)
        {
            gene[0] = 0;
            gene[1] = 1;
        }else if(this.option == 3)
        {
            gene[0] = 1;
            gene[1] = 1;
        }else if(this.option == 4)
        {
            gene[0] = 1;
            gene[1] = 0;
        }
    }

    private void genetooption()
    {
        if(gene[0] == 0 && gene[1] == 0)
        {
            this.option = 1;
        } else if (gene[0] == 0 && gene[1] == 1)
        {
            this.option = 2;
        }else if(gene[0] == 1 && gene[1] == 1)
        {
            this.option = 3;
        }else if(gene[0] == 1 && gene[1] == 0)
        {
            this.option = 4;
        }
    }

    public int getOption()
    {
        return this.option;
    }
}
