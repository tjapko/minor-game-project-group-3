using UnityEngine;
using System.Collections;

public class MudScript : MonoBehaviour {
	public float mudTime; //time mud is in the scene
	private UserObjectStatistics stats;

	// Use this for initialization
	void Start () {
		stats = gameObject.GetComponent<UserObjectStatistics> ();
		Invoke ("OnDeath", mudTime);
	}

	public void OnDeath(){
		stats.onDeath ();
		CancelInvoke ("OnDeath");
	}
}
