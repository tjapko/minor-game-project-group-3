using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyMovement Class
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    //Public variables
    public float m_speed;               //Speed of enemy
    public float m_turnspeed;           //Turnspeed of enemy
    [HideInInspector] public Transform m_targetlocation;     //Location of target (currently not looking for base but spawn of base)
   
    //Private variables
    private Rigidbody m_rigidbody;      //Rigid body 
    private Transform m_spawnlocation;  //Spawn location of enemy (might not be needed)
    

    //sets enemy to spawnpoint
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_spawnlocation = m_rigidbody.transform;
        gameObject.SetActive(true);
    }

    //moves enemy every update closer to tower by speed
    void FixedUpdate()
    {
			m_rigidbody.MovePosition (Vector3.MoveTowards (transform.position, m_targetlocation.position - new Vector3 (0, m_targetlocation.transform.position.y, 0), m_speed));
			turn ();
	}


    //Turns enemy facing the base
    void turn()
    {
        Vector3 turn = m_targetlocation.position - transform.position;
        turn.y = 0;
		if (turn == new Vector3(0,0,0)){
			return;
		}
        Quaternion rotation = Quaternion.LookRotation(turn);
        m_rigidbody.MoveRotation(rotation);
    }

    //Teleport enemy to spawn (volgens mij wordt dit door de game manager geregeld)
    void tospawnlocation()
    {
        transform.position = m_spawnlocation.position;
    }


}
