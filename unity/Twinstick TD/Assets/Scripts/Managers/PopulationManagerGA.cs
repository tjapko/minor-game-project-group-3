using UnityEngine;
using System.Collections;

/* to dos
    1 create a gen ( do this in the enemyinheratedvalues class(maybe change that class to gen))
        1.1 figure out how to encode the values to a gen and program it
        1.2 create a translation from the gen to enemyinherted values
    2 on creation
        2.1 create a enemy population the size of the wave for each enemy
        2.2 fill it with random generated genes
    3 create getters for each enemy type
    4 create an endwave function
        4.1 this first removes all non spawned enemys from its popluation
        4.2 then let it call the GA
    5 create the GA


    */
public class PopulationManagerGA {

    EnemyPopulation poptype1;
    EnemyPopulation poptype2;
    EnemyPopulation poptype3;
    EnemyInheratedValues boss;
    private GaneticAlgorithm GA = new GaneticAlgorithm();

    public PopulationManagerGA(float StartingAmountEnemies) { 
        poptype1 = new EnemyPopulation();
        poptype2 = new EnemyPopulation();
        poptype3 = new EnemyPopulation();
        createBoss();
        firstpop(StartingAmountEnemies, poptype1);
        firstpop(StartingAmountEnemies, poptype2);
        firstpop(StartingAmountEnemies, poptype3);
    }

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
