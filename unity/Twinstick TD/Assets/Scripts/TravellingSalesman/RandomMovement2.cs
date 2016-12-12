using UnityEngine;
using System.Collections;

public class RandomMovement2 : MonoBehaviour {

	public float timer;

	public int changeDirTime;

	public float speed;

	public NavMeshAgent nav;

	public Vector3 target;


	// Use this for initialization
	void Start () {
		nav = gameObject.GetComponent<NavMeshAgent> ();
	}

	void FixedUpdate () {
	
		timer += Time.deltaTime;
		if (timer >= changeDirTime) {
			newTarget ();
			timer = 0;
		}
	}

	private void newTarget() {
	
		float x = gameObject.transform.position.x;
		float z = gameObject.transform.position.z;

		x += Random.Range (x - 100, x + 100);
		z += Random.Range (z - 100, z + 100);

		target = new Vector3(x, 0.0f, z);

		nav.SetDestination (target);
	}
}
