using UnityEngine;
using System.Collections;

public class SettingsScript : MonoBehaviour {

    //References
    private GameManager m_gamemanager;  //Reference to game manager

	// Use this for initialization
	void StartInitialzation () {
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
