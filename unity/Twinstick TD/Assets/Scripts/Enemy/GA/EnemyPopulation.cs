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
        foreach(KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            if (!tempEnemy.Value)
            {
                return tempEnemy.Key;
            }
        }
        return PopulationList[0].Key;
    }

    public void clearUnspawnedEnemys()
    {
        List<KeyValuePair<EnemyInheratedValues, bool>> list = new List<KeyValuePair<EnemyInheratedValues, bool>>();
        foreach (KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            if (tempEnemy.Value)
            {
                list.Add(tempEnemy);
            }
        }
        PopulationList = list;
   }
    public void CalculateFitness()
    {
        totalFitness = 1;
        foreach (KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            totalFitness += tempEnemy.Key.getDamageDone();
        }
        //set fitness
        foreach (KeyValuePair<EnemyInheratedValues, bool> tempEnemy in PopulationList)
        {
            tempEnemy.Key.setFitness(tempEnemy.Key.getDamageDone() / totalFitness);
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

}