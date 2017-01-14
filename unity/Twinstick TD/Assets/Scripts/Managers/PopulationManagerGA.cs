using UnityEngine;
using System.Collections;

public class PopulationManagerGA {

    EnemyPopulation poptype1;
    EnemyPopulation poptype2;
    EnemyPopulation poptype3;

    public PopulationManagerGA(float StartingAmountEnemies) { 
        poptype1 = new EnemyPopulation();
        poptype2 = new EnemyPopulation();
        poptype3 = new EnemyPopulation();



    }

    public EnemyInheratedValues getStandartValues()
    {
        EnemyInheratedValues values = new EnemyInheratedValues(0.5f,2,1,2,2,20);

        return values;
    }

    public EnemyPopulation firstpop(float StartingAmountEnemies, EnemyPopulation pop, int enemytype)
    {


        if(enemytype == 3)
        {
            //EnemyManager enemy = new Enemie3();
        }else if(enemytype == 2)
        {
            
        }
        else {

        }


        for(int i = 0; i < StartingAmountEnemies; i++)
        {

        }

        return pop;
    }
    

}
