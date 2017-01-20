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
    private float baseAttackSpeedObject = 3f;
    private float baseDamageToPlayerPerAttack = 1f;
    private float baseAttackSpeedPlayer = 3f;
    private float baseStartingHealth = 1f;
    private float baseMovementspeed = 3f;


    private int geneamount = 20;
    private Chromosome chromosome;

	private int AimingEndWavenumber = 15;

	private float scaleDamagePerAttack = 2f;
	private float scaleAttackSpeed = 1f;
	private float scaleStartingHealth = 2f;
	private float scaleMovementSpeed = 2f;

	private float rangeDamagePerAttack = 2f; 
	private float rangeAttackSpeed = 1f;    
    private float rangeStartingHealth = 2f;  
	private float rangeMovementSpeed = 2f;  

	private float rangeDamagePerAttackDuplicate; 
	private float rangeAttackSpeedDuplicate;     
	private float rangeStartingHealthDuplicate;  
	private float rangeMovementSpeedDuplicate;   

    private float DamageDone = 0;
    private float Fitness = 0;

	public EnemyInheratedValues(int waveNumber)
    {
		updateRange (waveNumber);
        
        this.chromosome = new Chromosome(this.geneamount);
        this.chromosome.createRandomChromosome();
        ChromosomeToValues();

		rangeDamagePerAttackDuplicate = rangeDamagePerAttack; 
		rangeAttackSpeedDuplicate = rangeAttackSpeed;     
		rangeStartingHealthDuplicate = rangeStartingHealth;  
		rangeMovementSpeedDuplicate = rangeMovementSpeed;
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
        this.attackSpeedObject = this.baseAttackSpeedObject - attackspeed;
        this.attackSpeedPlayer = this.baseAttackSpeedPlayer - attackspeed;

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

	public void updateRange(int waveNumber)
	{
		if (waveNumber > 2) {
			this.rangeDamagePerAttack = this.rangeDamagePerAttackDuplicate * this.scaleDamagePerAttack / this.AimingEndWavenumber*waveNumber;

			this.rangeAttackSpeed = this.rangeAttackSpeedDuplicate * this.scaleAttackSpeed / this.AimingEndWavenumber*waveNumber;

			this.rangeStartingHealth = this.rangeStartingHealthDuplicate * this.scaleStartingHealth / this.AimingEndWavenumber*waveNumber;

			this.rangeMovementSpeed = this.rangeMovementSpeedDuplicate * this.scaleMovementSpeed / this.AimingEndWavenumber*waveNumber;
		
		}
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

