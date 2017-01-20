using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPopulation
{
    private List<KeyValuePair<EnemyInheratedValues, bool>> PopulationList;
    private float totalFitness;

    

    public EnemyPopulation()
    {
        PopulationList = new List<KeyValuePair<EnemyInheratedValues, bool>>();

    }

    public void AddEnemy(EnemyInheratedValues enemyInheratedValues)
    {
        this.PopulationList.Add(new KeyValuePair<EnemyInheratedValues, bool>(enemyInheratedValues, false));
    }



    public EnemyInheratedValues GetEnemyValues()
    {

        for( int i = 0; i < PopulationList.Count; i++)
        {
            if (!PopulationList[i].Value)
            {
                EnemyInheratedValues tempEnemy = PopulationList[i].Key;
                PopulationList[i] = new KeyValuePair<EnemyInheratedValues, bool>(tempEnemy, true);
                return tempEnemy;
            }
        }
  
        return PopulationList[0].Key;
        
    }

    public void clearUnspawnedEnemys()
    {
                PopulationList.RemoveAll(item => item.Value == false);
   }
    public void CalculateFitness()
    {
        totalFitness = 0;
        foreach (KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            float Damage = tempEnemy.Key.getDamageDone();
            if(Damage == 0)
            {
                Damage = 0.1f;
            }
            totalFitness += Damage;
        }
        //set fitness
        foreach (KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            float Damage = tempEnemy.Key.getDamageDone();
            if(Damage == 0)
            {
                Damage = 0.1f;
            }
            tempEnemy.Key.setFitness(Damage / totalFitness);
        }
    }
    public List<KeyValuePair<EnemyInheratedValues, bool>> getList()
    {
        return this.PopulationList;
    }

    public void AddAndShuffle(EnemyPopulation newpop)
    {
        foreach( KeyValuePair<EnemyInheratedValues, bool> Enemy in newpop.getList()){
            PopulationList.Add(Enemy);
        }
        for(int i = 0; i<PopulationList.Count; i++)
        {
            KeyValuePair<EnemyInheratedValues, bool> temp = PopulationList[i];
            int randomIndex = Random.Range(i, PopulationList.Count);
            PopulationList[i] = PopulationList[randomIndex];
            PopulationList[randomIndex] = temp;
        }
    }


    public void debugAverageStats()
    {

        float[] averageStats = new float[4];
        int counter = 0;
        foreach (KeyValuePair<EnemyInheratedValues, bool> Enemy in PopulationList){
            averageStats[0] += Enemy.Key.getDamageToObjectPerAttack();
            averageStats[1] += Enemy.Key.getAttackSpeedObject();
            averageStats[2] += Enemy.Key.getStartingHealth();
            averageStats[3] += Enemy.Key.getMovementspeed();
            counter++;
        }

        averageStats[0] = averageStats[0] / (float)counter;
        averageStats[1] = averageStats[1] / (float)counter;
        averageStats[2] = averageStats[2] / (float)counter;
        averageStats[3] = averageStats[3] / (float)counter;
        //Debug.Log("average Damage per attack = " + averageStats[0] + " average Attackspeed = " + averageStats[1] + " average Health = " + averageStats[2] + " average MovementSpeed = " + averageStats[3]);
        
    }
}