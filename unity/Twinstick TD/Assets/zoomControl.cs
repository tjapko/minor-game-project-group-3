using UnityEngine;
using System.Collections;

public class zoomControl : MonoBehaviour {

	private Camera m_constructionCam;
	private Vector3 m_CamPos;
	private float camRayLength = 100f;
	private int Floor;   

	// Use this for initialization
	void Start () {
		m_constructionCam = GetComponent<Camera> ();
		m_CamPos = m_constructionCam.transform.position;
		Floor = LayerMask.GetMask ("mouseFloor");
	}
	
	// Update is called once per frame
	void Update () {

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0)
		{
			if (m_constructionCam.orthographicSize > 40) {
				m_constructionCam.orthographicSize -= 20;
				m_constructionCam.transform.position = mousePos ();
			}
//			m_constructionCam.transform.position = mousePos ();
		}
		
		if (scroll < 0)
		{
			if (m_constructionCam.orthographicSize <= 60) {
				m_constructionCam.orthographicSize = 70;
				m_constructionCam.transform.position = mousePos ();
			} else {
				m_constructionCam.transform.position = m_CamPos;
				m_constructionCam.orthographicSize = 70;
			}
		}
	}

	// Adjust the rotation of the player based on the mousePosition input.
	private Vector3 mousePos()
	{
		// creating a ray from the camera to the mouseposition
		Ray camRay = m_constructionCam.ScreenPointToRay(Input.mousePosition);

		// a variable, which is true when the ray hits the floor 
		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, Floor)) // checking if the ray hits the floor 
		{
			Vector3 playerToMouse = floorHit.point - transform.position; // creating a vector from the player to the mousePosition
			playerToMouse.y = 0f; // because it's a projection on the x-z-plane
			playerToMouse = transform.position + playerToMouse;  //Used for dropping objects
			return playerToMouse;
		}
		return Vector3.zero;
	}
}
