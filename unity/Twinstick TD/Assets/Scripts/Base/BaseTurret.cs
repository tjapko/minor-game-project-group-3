﻿using UnityEngine;
using System.Collections;

public class BaseTurret : MonoBehaviour {

    //References
    [Header("References")]
    public GameObject m_bulletPrefab; //Reference to bullet prefab
    public Transform m_turretFireTransform; //Reference to the fire transform
    public GameObject m_turretbarrel;   //Reference to barrel of turret

    //Public variables
    [Header("Public variables")]
    public static float m_damage = 3f;    //damage of the turret
	public static float m_range = 25f;     //Range of the turret
	public static float m_accuracy = 3000f; //Accurracy of tower +/- (1/m_accuracy)
	public static float m_fireRate = 0.5f;   //Fire rate of the turret
	public static float m_launchspeed = 200f;   //Launch speed of the bullet
	public static float m_turnrate = 3f;   //Turn rate of the turret
	// damage, 

    //Private variables
    private GameObject m_target;        //Target (gameobject)
    private Transform m_targetlocation; //Location of target
    private float m_firecountdown;      //Countdown until next shot
    private Transform m_rotation;       //Rotation of turret
    private int m_PlayerNumber = 0;     //FIX PLZ
	private float m_FreqInvoke = 0.1f;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("getTarget", 0f, m_FreqInvoke); 
    }

    // Update is called once per frame
    void Update()
    {
        if (m_target != null)
        {
            TurnTurret();

            if (m_firecountdown > 1 / m_fireRate)
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
		BulletExplosion bullet_script = newbullet.GetComponent<BulletExplosion>();
		bullet_script.m_MaxDamage = m_damage;
		bullet_script.setPlayernumber((m_PlayerNumber));

		float acc_factor = 1 / m_accuracy;
		Vector3 bullet_direction = m_turretFireTransform.forward;
		bullet_direction.x += Random.Range(-acc_factor, acc_factor);
		bullet_direction.y += Random.Range(-acc_factor, acc_factor);
		bullet_direction.z += Random.Range(-acc_factor, acc_factor);
		bullet_direction.Normalize();
		bullet_direction = bullet_direction * m_launchspeed;
		newbullet.GetComponent<Rigidbody>().velocity = bullet_direction;
    }

    //Function to turn turret
    private void TurnTurret()
    {
        if (m_target != null)
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
	
    //Setter Set damage	
	public static void setDamage(float amount) {
		m_damage = amount;
	}

    //Setter Get Damage
	public static float getDamage() {
		return m_damage;
	}

    //Setter Range
	public static void setRange(float amount) {
		m_range = amount;
	}

    //Setter Accuracy
	public static void setAccuracy(float amount) {
		m_accuracy = amount;
	}

    //Setter Fire rate
	public static void setFirerate(float amount) {
		m_fireRate = amount;
	}

    //Setter launch force
	public static void setLaunchForce(float amount) {
		m_launchspeed = amount;
	}

    //Setter turn rate
	public static void setTurnRate(float amount) {
		m_turnrate = amount;
	}
		
}