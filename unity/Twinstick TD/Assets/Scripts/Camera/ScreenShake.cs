using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

	public static ScreenShake instance; 	// used in order to make Shake a static function!
	public static float shakeFreq = 10; 	// # shakes per s
	[HideInInspector]public static float screenShakeIntensityBase = 0.3f;
	[HideInInspector]public static float screenShakeDurationBase = 0.3f;
	[HideInInspector]public static float screenShakeIntensityCharacter = 0.2f;
	[HideInInspector]public static float screenShakeDurationCharacter = 0.3f;

	private static float shakeIntensity;	// amount of added offset to x-z-positions
	private static Camera cam;
	private static Vector3 camPos; 
	private static int shakingNumber = 0;
	private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.

	// Use this for initialization
	void Start () {
		cam = GetComponentInParent<Camera> ();
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.T)) {
			Shake (screenShakeIntensityBase, screenShakeDurationBase);
//			Shake2();
//			Shake3();
		}
		if (Input.GetKeyDown (KeyCode.Y)) {
			Shake (screenShakeIntensityCharacter, screenShakeDurationCharacter);
		}
	}

	// handles the starting and stopping of the screenShake
	public static void Shake(float intensity, float duration) {
		// stores the position of the camera before shaking 
		shakingNumber++;
		camPos = cam.transform.position;
		shakeIntensity = intensity;

		if (shakingNumber == 1) {
			if (instance) {
				instance.InvokeRepeating ("StartShake", 0, 1 / shakeFreq); 
				instance.Invoke ("StopShake", duration);
				shakingNumber = 0;
			}
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
//		cam.transform.position = camPos;

//		m_DesiredPosition = ;
//		cam.transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, 0.2f);
	}

	public static void ShakeBase() {
		Shake(screenShakeIntensityBase, screenShakeDurationBase);
	}

	public static void ShakeCharacter() {
		Shake(screenShakeIntensityCharacter, screenShakeDurationCharacter);
	}

}
