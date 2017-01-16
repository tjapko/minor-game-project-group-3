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
			Shake (0.2f, 0.3f);
//			Shake2();
//			Shake3();
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			Shake (0.1f, 0.2f);
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

	/*
	IEnumerator Shake2() {
		float duration = 0.5f;
		float magnitude = 0.5f;

		float elapsed = 0.0f;

		Vector3 originalCamPos = cam.transform.position;

		while (elapsed < duration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / duration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float z = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			z *= magnitude * damper;

			cam.transform.position = new Vector3(x, originalCamPos.y, z);

			yield return null;
		}

		cam.transform.position = originalCamPos;
	}
	*/

	/*
	public void Shake3() {
		Vector3 camPos = cam.transform.position;
		float amplitude = 0.1f;
		cam.transform.localPosition = camPos + Random.insideUnitSphere * amplitude;
	}
	*/
}
