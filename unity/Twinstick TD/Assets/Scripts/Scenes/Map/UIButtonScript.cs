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
        GameObject m_root = GameObject.FindWithTag("Gamemanager");
        m_gamemanager = m_root.GetComponent<GameManager>();
    }
	
    //Function for nextwave button
	public void BTN_nextwave()
    {
        m_gamemanager.btn_nextwave();
    }

    //Button for placing object
    public void BTN_construction_1()
    {
        StartCoroutine(m_gamemanager.getObjectManager().ObjectPlacement());
    }
}
