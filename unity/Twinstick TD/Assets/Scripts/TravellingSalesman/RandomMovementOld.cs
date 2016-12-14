using UnityEngine;
using System.Collections;

/// <summary>
/// Random movement old. NOT USED ANYMORE!
/// </summary>
public class RandomMovementOld : MonoBehaviour {

	public float m_minForce = 10.0f;
	public float m_maxForce = 50.0f;
	public float changeDirInterval = 1.0f;

	private UserManager player;
	private bool metPlayer = false; // boolean for registerating if player met the TravelingSalesman, default is false

	Rigidbody rb;

	private float force;
	private float x;
	private float z;
	private float changeDir; 

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>(); 
		changeDir = changeDirInterval;
		Move ();
	}
		
	void FixedUpdate () {
		changeDir -= Time.deltaTime;
		if (changeDir < 0) {
			Move ();
			changeDir = changeDirInterval; 
		}

//		if (metPlayer) {
//			RandomMovement.Destroy ();
//		}
	}

	// moves the TravellingSalesman into a new direction 
	void Move () {
		force = Random.Range (m_minForce, m_maxForce);
		x = Random.Range (-1.0f, 1.0f); 
		z = Random.Range (-1.0f, 1.0f); 

		// Vector3 Force = new Vector3 (x, 0.0f, z);
		Vector3 Force = new Vector3 (x, 0.0f, z);
		Force.Normalize();

		rb.AddForce (force * Force); // Transform.position instead of AddForce!
//		Vector3 vec = new Vector3(Random.Range(-50,50), 0.0f, Random.Range(-50,50));
//		rb.MovePosition(Vector3.MoveTowards(transform.position, vec, 1.0f));
	}
}
	