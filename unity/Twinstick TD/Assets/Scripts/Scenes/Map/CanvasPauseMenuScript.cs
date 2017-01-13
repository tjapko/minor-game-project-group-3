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
    private GameObject m_pausemenu;     //Reference to the pausemenu window (child of this GameObject)
    private GameObject m_controlsmenu;  //Reference to the controlsmenu window (child of this GameObject)        

    private void Start()
    {
        //Find gamemanager
        m_gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<GameManager>();
        m_usermanager = m_gamemanager.getUserManager();
        m_players = m_usermanager.m_playerlist;
        m_wavemanager = m_gamemanager.getWaveManager();

        //Set references to children
        m_pausemenu = gameObject.transform.GetChild(0).gameObject;
        m_controlsmenu = gameObject.transform.GetChild(1).gameObject;

        //Enable menus
        m_pausemenu.SetActive(true);
        m_controlsmenu.SetActive(false);

    }

    //On Enable function
    void OnEnable()
    {
        showPauseMenu(true);
        showControlsMenu(false);
    }

    //On disable function
    void OnDisable()
    {
        showPauseMenu(true);
        showControlsMenu(false);
    }

    // Show/hide Pause menu
    public void showPauseMenu(bool status)
    {
        if(m_pausemenu != null)
        {
            m_pausemenu.SetActive(status);
        }
    }

    // Show/hide Controls menu
    public void showControlsMenu(bool status)
    {
        if(m_controlsmenu != null)
        {
            m_controlsmenu.SetActive(status);
        }
    }
    
}
