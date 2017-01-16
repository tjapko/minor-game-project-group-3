using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public float shakeFreq = 100; // # shakes per s

	private float shakeIntensity;
	private Camera cam;
	private Vector3 camPos; 

	// Use this for initialization
	void Start () {
		cam = GetComponentInParent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			Shake (0.4f, 0.3f);
		}
	}

	public void Shake(float intensity, float duration) {
		camPos = cam.transform.position;
		shakeIntensity = intensity;

		InvokeRepeating ("StartShake", 0, 1/shakeFreq); 
		Invoke ("StopShake", duration);

	}

	// shaking the camera 
	public void StartShake() {
		// initial camera's position before the shaking starts
		Vector3 pos = cam.transform.position;

		// random shake offsets: 
		float shakeX = Random.value * shakeIntensity * 2 - shakeIntensity; // formula for shaking  map to -1, 1
		float shakeZ = Random.value * shakeIntensity * 2 - shakeIntensity; // formula for shaking 
		// adjusting position with shake offset 
		pos.x += shakeX;
		pos.z += shakeZ;
		// changing camera position: 
		cam.transform.position = pos;

	}

	// stopping the shaking of the main camera
	public void StopShake() {
		CancelInvoke ("StartShake");
		cam.transform.position = camPos;
//		cam.transform.localPosition = Vector3.zero;
	}
}
