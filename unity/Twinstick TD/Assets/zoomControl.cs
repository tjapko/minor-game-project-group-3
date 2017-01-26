using UnityEngine;
using System.Collections;

public class zoomControl : MonoBehaviour {

	private Camera m_constructionCam;
	private Vector3 m_CamPos;

	// Use this for initialization
	void Start () {
		m_constructionCam = GetComponent<Camera> ();
		m_CamPos = m_constructionCam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0)
		{
			m_constructionCam.orthographicSize = 30;
//			m_constructionCam.transform.position = new Vector3 (Input.mousePosition.x, m_CamPos.y, Input.mousePosition.z);
		}
		
		if (scroll < 0)
		{
			m_constructionCam.transform.position = m_CamPos;
			m_constructionCam.orthographicSize = 70;
		}
	
	}
}
