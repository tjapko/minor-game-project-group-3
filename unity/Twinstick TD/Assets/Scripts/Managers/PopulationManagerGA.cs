using UnityEngine;
using System.Collections;

/*the population manger hold three lists of each enemy inherated values and a boolean which tells if they are spawned or not

    */
public class PopulationManagerGA {

    EnemyPopulation poptype1;
    EnemyPopulation poptype2;
    EnemyPopulation poptype3;
    EnemyInheratedValues boss;
    private GaneticAlgorithm GA = new GaneticAlgorithm();

    // initialize all the lists and create a GA
    public PopulationManagerGA(float StartingAmountEnemies) { 
        poptype1 = new EnemyPopulation();
        poptype2 = new EnemyPopulation();
        poptype3 = new EnemyPopulation();
        createBoss();
    //    firstpop(StartingAmountEnemies, poptype1);
   //     firstpop(StartingAmountEnemies, poptype2);
      //  firstpop(StartingAmountEnemies, poptype3);
    }


    /* goes to the next genaration of the GA, 
     * It does this by first clearing the unspawned enemys of each list,
     * then a check is preformed to make sure there are more then two spawned enemies to make sure there are parent avaible
     * then it provides the population to the GA and that returns a new populaiton
     */
    public void nextGenartion(int AmountEnemies)
    {
        

        poptype1.clearUnspawnedEnemys();
        poptype2.clearUnspawnedEnemys();
        poptype3.clearUnspawnedEnemys();
      
        // check for popsize
        if (poptype1.getList().Count > 2)
        {
            poptype1 = GA.RunGA(poptype1, AmountEnemies);
        }
        else
        {
            restockPop(poptype1, AmountEnemies);
        }
        if (poptype2.getList().Count > 2)
        {
            poptype2 = GA.RunGA(poptype2, AmountEnemies);
        }
        else
        {
            restockPop(poptype2, AmountEnemies);
        }
        if (poptype3.getList().Count > 2)
        {
            poptype3 = GA.RunGA(poptype3, AmountEnemies);
        }
        else
        {
            restockPop(poptype3, AmountEnemies);
        }

//        Debug.Log("type 1 = ");
//        poptype1.debugAverageStats();
//        Debug.Log("type 2 = ");
//        poptype2.debugAverageStats();
//        Debug.Log("type 3 = ");
//        poptype3.debugAverageStats();


    }

    public EnemyInheratedValues getInheratedType1()
    {
        EnemyInheratedValues values = poptype1.GetEnemyValues();

        return values;
    }
    public EnemyInheratedValues getInheratedType2()
    {
        EnemyInheratedValues values = poptype2.GetEnemyValues();

        return values;
    }
    public EnemyInheratedValues getInheratedType3()
    {
        EnemyInheratedValues values = poptype3.GetEnemyValues();

        return values;
    }

    public EnemyInheratedValues getInheratedType4()
    {
        return this.boss;
    }

    public void firstpop(float StartingAmountEnemies, EnemyPopulation pop)
    {
        for (int i = 0; i< StartingAmountEnemies; i++)
        {
            EnemyInheratedValues enemy = new EnemyInheratedValues();
            pop.AddEnemy(enemy);
            Debug.Log(pop.getList().Count);
        }
    }
    
    private void restockPop(EnemyPopulation pop, float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            EnemyInheratedValues enemy = new EnemyInheratedValues();
            pop.AddEnemy(enemy);
        }
    }

    private void createBoss()
    {
        boss = new EnemyInheratedValues();
        boss.isBoss(10f,10f,10f,10f,10f,30f);
    }
}
