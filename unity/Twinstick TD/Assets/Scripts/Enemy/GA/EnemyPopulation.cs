using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyPopulation
{
    private List<KeyValuePair<EnemyInheratedValues, bool>> PopulationList;

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
}