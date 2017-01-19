using UnityEngine;
using System.Collections;

public class EnemyInheratedValues  {

    private float damageToObjectPerAttack;
    private float attackSpeedObject;
    private float damageToPlayerPerAttack;
    private float attackSpeedPlayer;
    private float StartingHealth;
    private float movementspeed;


    private float baseDamageToObjectPerAttack = 1f;
    private float baseAttackSpeedObject = 1f;
    private float baseDamageToPlayerPerAttack = 1f;
    private float baseAttackSpeedPlayer = 1f;
    private float baseStartingHealth = 2f;
    private float baseMovementspeed = 6f;


    private int geneamount = 20;
    private Chromosome chromosome;

    private float rangeDamagePerAttack = 5f;
    private float rangeAttackSpeed = 5f;
    private float rangeStartingHealth =10f;
    private float rangeMovementSpeed = 5f;

    private float DamageDone = 0;
    private float Fitness = 0;

	public EnemyInheratedValues()
    {
     
        
        chromosome = new Chromosome(geneamount);
        chromosome.createRandomChromosome();
        ChromosomeToValues();
    }

    public void ChromosomeToValues()
    {
        chromosome.setVarriables();

        // damage per attack
        float damageperAttack = (chromosome.getAmountOption1()/(float)geneamount)*this.rangeDamagePerAttack;
        this.damageToObjectPerAttack = this.baseDamageToObjectPerAttack + damageperAttack;
        this.damageToPlayerPerAttack = this.baseDamageToPlayerPerAttack + damageperAttack;

        // attack speed
        float attackspeed = (chromosome.getAmountOption2() / (float)geneamount) * this.rangeAttackSpeed;
        this.attackSpeedObject = this.baseAttackSpeedObject + attackspeed;
        this.attackSpeedPlayer = this.baseAttackSpeedPlayer + attackspeed;

        // health
        float health = (chromosome.getAmountOption3() / (float)geneamount) * this.rangeStartingHealth;
        this.StartingHealth = this.baseStartingHealth + health;

        //movementspeed
        float movement = (chromosome.getAmountOption4() / (float)geneamount) * this.rangeMovementSpeed;
        this.movementspeed = this.baseMovementspeed + movement;

    }

    public void mutate()
    {
        chromosome.mutate();
        ChromosomeToValues();
    }


    public void setDamageDone(float damageToObject, float damageToPlayer)
    {
        DamageDone = damageToObject + damageToPlayer;

    }

    public void setFitness(float fitness)
    {
        this.Fitness = fitness;
    }
    
    public float getFitness()
    {
        return this.Fitness;
    }

    public float getDamageDone()
    {
        return this.DamageDone;
    }

    public Chromosome getChromosome()
    {
        return this.chromosome;
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
        return this.damageToPlayerPerAttack;
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

  

    public void isBoss(float damageObject, float speedObject, float damagePlayer, float speedPlayer, float Health, float move)
    {
        this.damageToObjectPerAttack = damageObject;
        this.attackSpeedObject = speedObject;
        this.damageToPlayerPerAttack = damagePlayer;
        this.attackSpeedPlayer = speedPlayer;
        this.StartingHealth = Health;
        this.movementspeed = move;
    }
}

