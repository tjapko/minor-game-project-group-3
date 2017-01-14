using UnityEngine;
using System.Collections;

public class EnemyInheratedValues  {

    private float damageToObjectPerAttack;
    private float attackSpeedObject;
    private float damgeToPlayerPerAttack;
    private float attackSpeedPlayer;
    private float StartingHealth;
    private float movementspeed;

	public EnemyInheratedValues(float damageToObjectPerAttack, float attackSpeedObject, float damageToPlayerPerAttack, float attackSpeedPlayer, float StartingHealth, float movementspeed)
    {
        this.damageToObjectPerAttack = damageToObjectPerAttack;
        this.attackSpeedObject = attackSpeedObject;
        this.damgeToPlayerPerAttack = damageToPlayerPerAttack;
        this.attackSpeedPlayer = attackSpeedPlayer;
        this.StartingHealth = StartingHealth;
        this.movementspeed = movementspeed;
    }

    public float getDamageToObjectPerAttack()
    {
        return this.damageToObjectPerAttack;
    }

    public float getAttackSpeedObject()
    {
        return this.attackSpeedObject;
    }

    public float getDamgeToPlayerPerAttack()
    {
        return this.damgeToPlayerPerAttack;
    }

    public float GetAttackSpeedPlayer()
    {
        return this.attackSpeedPlayer;
    }


    public float getStartingHealth()
    {
        return this.StartingHealth;
    }

    public float getMovementspeed()
    {
        return this.movementspeed;
    }
}

