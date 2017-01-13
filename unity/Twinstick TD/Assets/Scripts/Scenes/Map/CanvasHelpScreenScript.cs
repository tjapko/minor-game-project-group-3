using UnityEngine;
using System.Collections;

public class CanvasHelpScreenScript : MonoBehaviour {
    //Public Variables
    public float m_lifetime;    //Life time of help screen

	// Use this for initialization
	void Start () {
        Destroy(gameObject, m_lifetime);
	}

    //Destroy the Help screen
    public void destroyObject()
    {
        Destroy(gameObject);
    }
	
}
