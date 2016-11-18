using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
	private Rigidbody rb;
	public float speed;
	public Transform spawn;
	public Transform tower;

	//sets enemy to spawnpoint
	void Start () {
		transform.position = spawn.position;
	}

	//moves enemy every update closer to tower by speed
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, tower.position - new Vector3 (0, tower.transform.position.y, 0), speed);
	}
}
