using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurretScript : MonoBehaviour {

    //References
    [Header("References")]
    public GameObject m_bulletPrefab; //Reference to bullet prefab
    public Transform m_turretFireTransform; //Reference to the fire transform
    public GameObject m_turretbarrel;   //Reference to barrel of turret
	public GameObject m_hitBaseCanvasPrefab;    //Reference to friendlyHit canvas;
	public Slider m_Slider;
	public Image m_FillImage;

    //Public variables
    [Header("Public variables")]
    public float m_damage = 100f;    //damage of the turret
    public float m_range = 100f;     //Range of the turret
    public float m_launchspeed = 10f;   //Launch speed of the bullet
    public float m_fireRate = 1f;   //Fire rate of the turret
    public float m_turnrate = 1f;   //Turn rate of the turret
    public float m_accuracy = 1000f; //Accurracy of tower +/- (1/m_accuracy)
	public float m_startHealth;
	public Color m_FullHealthColor = Color.green;   //Full health colour
	public Color m_ZeroHealthColor = Color.red;     //Zero health colour

    //Private variables
    private GameObject m_target;        //Target (gameobject)
    private Transform m_targetlocation; //Location of target
    private float m_firecountdown;      //Countdown until next shot
    private Transform m_rotation;       //Rotation of turret
    private int m_PlayerNumber;
	private float m_currentHealth;
	private bool m_Dead;
	private UserObjectStatistics stats;

    // Use this for initialization
    void Start () {
		stats = gameObject.GetComponent<UserObjectStatistics> ();
        InvokeRepeating("getTarget", 0f, 0.5f);
		m_currentHealth = m_startHealth;
		m_Dead = false;
		SetHealthUI ();
        PlayerConstruction pl = new PlayerConstruction();
        pl.currentTurrets++;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_target != null)
        {
            TurnTurret();

            if(m_firecountdown > 1/m_fireRate)
            {
                Fire();
                m_firecountdown = 0;
            }
        }
        m_firecountdown += Time.deltaTime;
    }

    // Function to get target
    private void getTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject targetenemy = null;
        float min_distance = m_range;
        
        foreach (GameObject enemy in enemies)
        {
            float distance_enemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance_enemy < min_distance)
            {
                min_distance = distance_enemy;
                targetenemy = enemy;
            }
        }

        if (targetenemy != null)
        {
            m_target = targetenemy;
        }
        else
        {
            m_target = null;
        }

    }

    //Function to fire bullet
    private void Fire()
    {
        GameObject newbullet = GameObject.Instantiate(m_bulletPrefab, m_turretFireTransform.position, m_turretFireTransform.rotation) as GameObject;
        float acc_factor = 1 / m_accuracy;
        Vector3 bullet_direction = m_turretFireTransform.forward;
        bullet_direction.x += Random.Range(-acc_factor, acc_factor);
        bullet_direction.y += Random.Range(-acc_factor, acc_factor);
        bullet_direction.z += Random.Range(-acc_factor, acc_factor);
        bullet_direction.Normalize();
        bullet_direction = bullet_direction* m_launchspeed;
        newbullet.GetComponent<Rigidbody>().velocity = bullet_direction;

        newbullet.gameObject.GetComponent<BulletExplosion>().setPlayernumber((m_PlayerNumber));
    }

    //Function to turn turret
    private void TurnTurret()
    {
        if(m_target != null)
        {
            Vector3 dir = m_target.transform.position - m_turretFireTransform.transform.position;
            Quaternion dir_to_face = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(m_turretFireTransform.transform.rotation, dir_to_face, Time.deltaTime * m_turnrate).eulerAngles;
            m_turretbarrel.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);
        }
        
    }

    //Shows the range of the turret
    public void showRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }

    //Set player number (passed through bullet)
    public void setPlayerNumber(int number)
    {
        m_PlayerNumber = number;
    }

	public void takeDamage(float damage){
		//Set hitmark
		createHitMark(m_hitBaseCanvasPrefab, damage);

		// Reduce current health by the amount of damage done.
		m_currentHealth -= damage;
		// Change the UI elements appropriately.
		SetHealthUI();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (m_currentHealth <= 0f && !m_Dead)
		{
			//CancelInvoke ();
			OnDeath();
		}
	}

	private void SetHealthUI()
	{
		// Set the slider's value appropriately.
		m_Slider.value = m_currentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_currentHealth / m_startHealth);
	}

	//Spawn hitmark
	private void createHitMark(GameObject prefab, float amount)
	{
		//Set hitmark
		GameObject hitbox = GameObject.Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		hitbox.GetComponent<HitMarkScript>().setDamage(amount);
	}

	// OnDeath
	private void OnDeath()
	{
		stats.onDeath ();
		m_Dead = true;
		//gameObject.SetActive(false);
        PlayerConstruction pl = new PlayerConstruction();
        pl.currentTurrets--; 
	}
}
