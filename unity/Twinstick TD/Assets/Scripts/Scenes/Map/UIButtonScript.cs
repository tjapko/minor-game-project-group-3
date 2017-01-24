using UnityEngine;
using System.Collections;

/// <summary>
/// Class UIButtonScript
/// Assigns commands to buttons
/// </summary>
public class UIButtonScript : MonoBehaviour {

    private GameManager m_gamemanager;

	// Use this for initialization
	void Start () {

    }
	
    //Function for nextwave button
	public void BTN_nextwave()
    {
        m_gamemanager.btn_nextwave();
    }

    //Function pause button
    public void BTN_setpause(bool status)
    {
        m_gamemanager.setpause(status);
    }

    //Function restart button
    public void BTN_restart()
    {
        m_gamemanager.btn_restartgame();
    }
    //Button for placing object
    //public void BTN_construction_1()
    //{
    //    StartCoroutine(m_gamemanager.getObjectManager().ObjectPlacement());
    //}


}
