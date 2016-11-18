using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed;
	public GameObject spawn;

	//sets player to spawnpoint
	void Start(){
		transform.position = spawn.transform.position;
	}

	//lets player turn and move
	void FixedUpdate(){
		float turnHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (0, 0, moveVertical);
		Vector3 rotation = new Vector3 (0, turnHorizontal*10, 0);
		movement *= Time.deltaTime * speed;
		rotation *= Time.deltaTime * speed;
		transform.Translate(movement);
		transform.Rotate (rotation);
	}
}
