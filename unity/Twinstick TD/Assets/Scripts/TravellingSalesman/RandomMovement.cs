using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {

	public float m_minForce = 10.0f;
	public float m_maxForce = 50.0f;
	public float changeDirInterval = 1.0f;

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

	// Update is called once per frame
	void Update () {
		changeDir -= Time.deltaTime;
		if (changeDir < 0) {
			Move ();
			changeDir = changeDirInterval; 
		}
	}

	// moves the TravellingSalesman into a new direction 
	void Move () {
		force = Random.Range (m_minForce, m_maxForce);
		x = Random.Range (-1.0f, 1.0f); 
		z = Random.Range (-1.0f, 1.0f); 

		//		Vector3 Force = new Vector3 (x, 0.0f, z);
		Vector3 Force = new Vector3 (x, 0.0f, z);

		rb.AddForce (force * Force); // Transform.position instead of AddForce!
	}
}
