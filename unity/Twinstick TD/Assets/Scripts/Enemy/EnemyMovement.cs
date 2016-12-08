using System;
using UnityEngine;

public class EnemyMovement: MonoBehaviour
{
	[HideInInspector] public Transform player;
	float MoveSpeed = 4f;
	float MaxDist = 10f;
	float MinDist = 5f;


	private void FixedUpdate(){
		transform.LookAt(player);
		if (Vector3.Distance(transform.position, player.position) >= MinDist){
			transform.position+= transform.forward*MoveSpeed*Time.deltaTime;
		}

		if (Vector3.Distance(transform.position, player.position) <= MaxDist){
			//Here call to shoot
		}
	}
}
