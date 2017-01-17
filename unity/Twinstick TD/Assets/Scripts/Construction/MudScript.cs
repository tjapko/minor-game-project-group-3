using UnityEngine;
using System.Collections;

public class MudScript : MonoBehaviour {
	public float mudTime; //time mud is in the scene

	// Use this for initialization
	void Start () {
		Invoke ("OnDeath", mudTime);
	}

	public void OnDeath(){
		CancelInvoke ("OnDeath");
		Destroy (gameObject);
	}
}
