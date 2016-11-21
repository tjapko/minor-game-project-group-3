using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	private Rigidbody rb;						//rigidbody used by enemy to move
	public float speed;							//speed of enemy
	[HideInInspector] public Transform tower;	//Location of base

	//sets enemy to spawnpoint
	void Start () {
		rb = GetComponent<Rigidbody> ();
		gameObject.SetActive (true);
	}

	//moves enemy every update closer to tower by speed
	void Update () {
		rb.MovePosition(Vector3.MoveTowards(transform.position, tower.position - new Vector3 (0, tower.transform.position.y, 0), speed));
		turn ();
	}

	//turns enemy facing the base
	void turn(){
		Vector3 turn = (tower.position - transform.position);
		turn.y = 0;
		Quaternion rotation = Quaternion.LookRotation(turn);
		rb.MoveRotation (rotation);
	}


}
