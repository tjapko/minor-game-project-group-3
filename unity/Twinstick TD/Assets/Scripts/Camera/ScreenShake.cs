using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public static ScreenShake instance; 	// used in order to make Shake a static function!
	public static float shakeFreq = 10; 	// # shakes per s

	private static float shakeIntensity;	// amount of added offset to x-z-positions
	private static Camera cam;
	private static Vector3 camPos; 

	// Use this for initialization
	void Start () {
		cam = GetComponentInParent<Camera> ();
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
//			Shake (0.2f, 0.3f);
//			Shake(0.1f, 0.2f); 
		}
	}

	// handles the starting and stopping of the screenShake
	public static void Shake(float intensity, float duration) {
		// stores the position of the camera before shaking 
		camPos = cam.transform.position;
		shakeIntensity = intensity;

		if (instance) {
			instance.InvokeRepeating ("StartShake", 0, 1/shakeFreq); 
			instance.Invoke ("StopShake", duration);
		}
	}

	// shaking the camera 
	public void StartShake() {
		// initial camera's position before the shaking starts
		Vector3 pos = cam.transform.position;

		// random shake offsets: 
		float shakeX = Random.value * shakeIntensity * 2 - shakeIntensity; // creating a small offsets for the x-axis
		float shakeZ = Random.value * shakeIntensity * 2 - shakeIntensity; // creating a small offsets for the z-axis
		// adjusting position with shake offset 
		pos.x += shakeX;
		pos.z += shakeZ;
		// changing camera position: 
		cam.transform.position = pos;

	}

	// stopping the shaking of the main camera
	public void StopShake() {
		if (instance) {
			instance.CancelInvoke ("StartShake");
		}
		cam.transform.position = camPos;
	}

}
