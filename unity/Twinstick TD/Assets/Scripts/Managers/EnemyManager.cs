using System;
using UnityEngine;


[Serializable]
public class EnemyManager
{
	public void OnTriggerEnter(Collider other){
		//if colide with base, damage base and set enemy to inactive
		if (other.gameObject.CompareTag ("Base")) {
			Rigidbody targetRigidbody = other.GetComponent<Rigidbody> ();
			if (targetRigidbody) {
				Basehealth basehealth = targetRigidbody.GetComponent<Basehealth> ();
				basehealth.TakeDamage (1f);
//				m_Instance.SetActive (false);				
			}
		}
	}
}
