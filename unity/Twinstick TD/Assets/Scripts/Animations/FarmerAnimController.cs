using UnityEngine;
using System.Collections;

public class FarmerAnimController : MonoBehaviour {

	static Animator anim;


	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if ((Input.GetAxisRaw("Vertical_1") != 0) || Input.GetAxisRaw("Horizontal_1") != 0) {
			anim.SetBool ("IsWalking", true);
		} else {
			anim.SetBool ("IsWalking", false);
		}
	}
}
