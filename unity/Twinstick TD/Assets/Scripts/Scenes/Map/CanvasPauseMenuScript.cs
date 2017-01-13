using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CanvasPauseMenuScript : MonoBehaviour {
    //References to managers
    private GameManager m_gamemanager;      //Reference to game manager (used to invoke next wave)
    private UserManager m_usermanager;      //Reference to the usermanager in the game manager
    private WaveManager m_wavemanager;
    private List<PlayerManager> m_players;  //Reference to the players in the game 

    //References to GameObject
    private GameObject m_pausemenu;            //Reference to the pausemenu window (child of this GameObject)

    private void Start()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Set references to children
        m_pausemenu = gameObject.transform.GetChild(0).gameObject;

    }

    //On disable function
    void OnDisable()
    {
        if(m_pausemenu != null)
        {
            showPauseMenu(true);
        }
    }

    // Show/hide Pause menu
    public void showPauseMenu(bool status)
    {
        m_pausemenu.SetActive(status);
    }

    
}
